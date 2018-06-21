// WcaProgammerConsoleUnmanaged.cpp : Defines the entry point for the console application.
//

#include "stdafx.h"
#include <iostream>
#include <iomanip>
#include "ProgrammingDevice.h"
#include "WCADevice.h"

using namespace std;

bool SendIPCCommand(Device * ecu, byte cmd, byte data[], int length);
void Byte2HexString(byte val, char*buffer);

const char* device_channel = "COM10";
const char* control_channel = "COM21";


int _tmain(int argc, _TCHAR* argv[])
{
	const int array_size = 20;
	byte receive_data[array_size];
	byte send_data[array_size];
	byte* p_receive_data = receive_data; 
	int written;
	int length;
	memset(receive_data, 0, array_size);
	bool result;


	ProgrammingDevice *progbox = new ProgrammingDevice(); //communication with programmer
	Device *ecu = new Device(0x01); //communication with test device

	//Press ProgStartButton of Programmer to hold device in bootloader
	progbox->Initialize(control_channel,9600);
	progbox->SimulatePressProgStartButton(300);
	progbox->DeInitialize();

	Sleep(250);
	////Start connection to Bootloader 
	result = ecu->Initialize(device_channel,9600);
	if (!result)
	{
		cout << "ecu->Initialize failed" << endl;
		goto DEINIT_APPLICATION;
	}
	
	send_data[0] = 0x01;
	send_data[1] = 0x03;
	send_data[2] = 0x7D;
	result = SendIPCCommand(ecu,0x24,send_data,3); //keep bootloader running
	if (!result)
	{
		cout << "SendIPCCommand 0x24 failed" << endl;
		goto DEINIT_APPLICATION;
	}


	result = SendIPCCommand(ecu,0x22,NULL,0); //read current running application / not needed, just for information
	if (!result)goto DEINIT_APPLICATION;

	send_data[0] = 0x02;
	send_data[1] = 0x07;
	send_data[2] = 0x00;
	result = SendIPCCommand(ecu,0x24,send_data,3); //start user application in EOL mode at 9600 bit/s
	if (!result)goto DEINIT_APPLICATION;

	
	ecu->DeInitialize();
	ecu->Initialize(device_channel,9600);

	Sleep(500); //wait until application has entirely been started 
	result = SendIPCCommand(ecu,0x26,NULL,0); //get current running application
	if (!result)goto DEINIT_APPLICATION;


	result = SendIPCCommand(ecu,0x1E,NULL,0); //Get user application version
	if (!result)goto DEINIT_APPLICATION;
	

DEINIT_APPLICATION:

	ecu->DeInitialize();
	
	
	delete ecu;
	delete progbox;
	return 0;
}

bool SendIPCCommand(Device * ecu, byte cmd, byte data[], int length)
{
	const int array_size = 20;
	byte receive_data[array_size];
	byte* p_receive_data = receive_data;
	int written;
	bool result = false;
	char cbuf[3];
	WcaInterfaceCommandResult wca_result;
	memset(receive_data, 0, array_size);

	Byte2HexString(cmd,cbuf);

	cout << "S:" << setw(2) << cbuf << ":";
	for (int i = 0; data != NULL && i < length; ++i)
	{	Byte2HexString(data[i],cbuf);
		cout << setw(2) << cbuf << " ";
	}
	cout << endl;

	wca_result = ecu->SendCommandSync_2(cmd,data,length,&p_receive_data,array_size,&written);

	if(wca_result== WcaInterfaceCommandResult_PosAck)
	{
		cout << "R:";
		for (int i = 0; i < written; ++i)
		{
			cout << hex << setfill('0') << setw(2) << (int)receive_data[i] << " ";
		}
		cout << endl;
		result = true;
	}
	else
	{
		cout << "ERROR" << endl;
	}
	return result;
}

void Byte2HexString(byte val, char* buffer)
{
	const char* pszNibbleToHex = {"0123456789abcdef"};
	//*buffer = (char *) malloc((2) + 1);
	int nibble = val>>4;
	buffer[0] = pszNibbleToHex[nibble];
	nibble = val & 0x0F;
	buffer[1] = pszNibbleToHex[nibble];
	buffer[2] = '\0';
}