#include "StdAfx.h"
#include "Device.h"

Device::Device(byte target_address)
{
	m_target_address = target_address;
	m_pInterface = new WcaInterfaceHandler();
}

Device::~Device(void)
{
	delete m_pInterface;
}

bool Device::Initialize(const char* pCOMPort, int baudrate)
{
	return m_pInterface->Initialize(pCOMPort, baudrate);
}

void Device::DeInitialize()
{
	m_pInterface->DeInitialize();
}

WcaInterfaceCommandResult Device::SendCommandSync(byte command, byte** prdata, int size, int *written)
{
	return m_pInterface->SendCommandSync(m_target_address,command,prdata,size,written);
}

WcaInterfaceCommandResult Device::SendCommandSync_2(byte command, byte data[], int length, byte** prdata, int size, int *written)
{
	return m_pInterface->SendCommandSync_2(m_target_address,command,data,length,prdata,size,written);
}

WcaInterfaceCommandResult Device::SetOutput(byte port, byte pin, byte value)
{
	WcaInterfaceCommandResult result;
	
	const int array_size = 20;
	byte rec_data[array_size];
	byte trans_data[array_size];
	byte* bp = rec_data; 
	int written;
	int length;
	memset(rec_data, 0, array_size);

	trans_data[0] = port; //Port
	trans_data[1] = pin;
	trans_data[2] = value;
	length = 3;

	result = SendCommandSync_2(0x2A,trans_data,length,&bp,array_size,&written);
	
	return result;
}

WcaInterfaceCommandResult Device::ReadInput(byte port, byte pin, byte *value)
{
	WcaInterfaceCommandResult result;
	
	const int array_size = 20;
	byte rec_data[array_size];
	byte trans_data[array_size];
	byte* bp = rec_data; 
	int written;
	int length;
	memset(rec_data, 0, array_size);

	trans_data[0] = port; //Port
	trans_data[1] = pin;
	length = 2;

	if(SendCommandSync_2(0x2C,trans_data,length,&bp,array_size,&written) == WcaInterfaceCommandResult_PosAck)
	{
		*value = rec_data[0];
		result = WcaInterfaceCommandResult_PosAck;
	}
	
	return result;
}
