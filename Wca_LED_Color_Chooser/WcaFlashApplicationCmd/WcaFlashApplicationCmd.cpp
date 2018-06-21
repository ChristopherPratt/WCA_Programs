// WcaProgammerConsoleUnmanaged.cpp : Defines the entry point for the console application.
//

#include "stdafx.h"
#include <iostream>
#include <iomanip>
#include <fstream>
#include <string>
#include "ProgrammingDevice.h"
#include "WCADevice.h"
#include "version.h"

using namespace std;

bool SendIPCCommand(Device * ecu, byte cmd, byte data[], int length, bool logging);
bool SendIPCCommand2(Device * ecu, byte cmd, byte data[], int length, byte** prdata, int size, int *written, bool logging);
void Byte2HexString(byte val, char*buffer);
bool UploadFile(Device * ecu, const char* full_prog_file_name);
void PrintInfo(bool error);

//const char* device_channel = "COM10";
//const char* control_channel = "COM21";


int main(int argc, const char* argv[])
{
	const int array_size = 20;
	byte receive_data[array_size];
	byte send_data[array_size];
	byte* p_receive_data = receive_data; 
	//int written;
	//int length;
	memset(receive_data, 0, array_size);
	bool result;


	if (argc != 3)
	{
		PrintInfo(true);
		return 1;
	}

	PrintInfo(false);

	//std::string control_channel(argv[1]);
	std::string device_channel(argv[1]);
	std::string full_prog_file_name(argv[2]);

	//ProgrammingDevice *progbox = new ProgrammingDevice(); //communication with programmer
	Device *ecu = new Device(0x01); //communication with test device

	//Press ProgStartButton of Programmer to hold device in bootloader
	//cout << "Initialize progbox" << endl;
	//progbox->Initialize(control_channel.c_str(),115200);
	//progbox->SimulatePressProgStartButton(300);
	

	//Sleep(250);
	////Start connection to Bootloader 
	cout << "Initialize device" << endl;
	result = ecu->Initialize(device_channel.c_str(),9600);
	if (!result)
	{
		cout << "ecu->Initialize failed" << endl;
		goto DEINIT_APPLICATION;
	}
	
	send_data[0] = 0x01;
	send_data[1] = 0x03;
	send_data[2] = 0x7D;
	result = SendIPCCommand(ecu,0x24,send_data,3,false); //keep bootloader running
	if (!result)
	{
		cout << "SendIPCCommand 0x24 failed" << endl;
		goto DEINIT_APPLICATION;
	}

	result = SendIPCCommand(ecu,0x1E,NULL,0,true); //Get user application version
	if (!result)goto DEINIT_APPLICATION;

	result = UploadFile(ecu, full_prog_file_name.c_str());
	if (!result)goto DEINIT_APPLICATION;

	result = SendIPCCommand(ecu,0x1E,NULL,0,true); //Get user application version
	if (!result)goto DEINIT_APPLICATION;
	
	cout << "Flashing successfully done." << endl;

DEINIT_APPLICATION:

	//progbox->DeInitialize();
	ecu->DeInitialize();
	
	delete ecu;
	//delete progbox;
	return 0;
}

bool UploadFile(Device * ecu, const char* full_prog_file_name)
{
	bool result = true;
	std::ifstream file(full_prog_file_name,std::ifstream::in);
	std::string str; 
	char sd[100];
	USHORT seg_cnt = 0;
	int char_cnt = 0;

	while (std::getline(file, str))
	{
		str.copy(sd+2,str.length(),0);
		sd[0] = seg_cnt >> 0*8;
		sd[1] = seg_cnt >> 1*8;

		sd[str.length()+2] = '\r';
		sd[str.length()+3] = '\n';

		if(!SendIPCCommand(ecu, 0x2E, (unsigned char*) sd, str.length()+4,false))
		{
			result = false;
			break;
		}
		else
		{
			if(char_cnt++ % 5 == 0) cout << ".";
		}
		seg_cnt++;
	}

	cout << endl;

	return result;
}

bool SendIPCCommand(Device * ecu, byte cmd, byte data[], int length, bool logging)
{
	const int array_size = 20;
	int written;
	byte receive_data[array_size];
	byte* p_receive_data = receive_data;
	memset(receive_data, 0, array_size);
	return SendIPCCommand2(ecu, cmd, data, length, &p_receive_data, array_size, &written, logging);
}

bool SendIPCCommand2(Device * ecu, byte cmd, byte data[], int length, byte** prdata, int size, int *written, bool logging)
{
	bool result = false;
	char cbuf[3];
	WcaInterfaceCommandResult wca_result;
	

	Byte2HexString(cmd,cbuf);

	if (logging)
	{
		cout << "S:" << setw(2) << cbuf << ":";
		for (int i = 0; data != NULL && i < length; ++i)
		{
			Byte2HexString(data[i], cbuf);
			cout << setw(2) << cbuf << " ";
		}
		cout << endl;
	}

	wca_result = ecu->SendCommandSync_2(cmd,data,length, prdata, size,written);

	if(wca_result== WcaInterfaceCommandResult_PosAck)
	{
		if (logging)
		{
			cout << "R:";
			for (int i = 0; i < *written; ++i)
			{
				cout /*<< hex*/ << setfill('0') << setw(2) << (int)(prdata[0][i]) << " ";
			}
			cout << endl;
		}
		result = true;
	}
	else
	{
		if (logging)
		{
			cout << "ERROR" << endl;
		}
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

void PrintInfo(bool error)
{
	cout << "Command line tool for flashing user application of Moray Ruby 1.2., 16.05.2017" << endl << endl;
	//cout << "WcaFlashApplicationCmd version: " << VER_FILE_VERSION_STR << endl << endl;

	if (error)
	{
		cout << "Start application as follow:" << endl;
		cout <<"WcaFlashApplicationCmd.exe <PORT Charger> <Full file name to be flashed>" << endl << endl;
		cout <<"e.g. WcaFlashApplicationCmd.exe COM10 \"f:\\projects\\git_projects\\moray_ruby_1_2\\releases\\MO_WC_11_1_8_3.S19\"" << endl;
	}
}