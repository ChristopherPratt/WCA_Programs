#pragma once


#include "WcaInterfaceHandler.h"

#define GPIO_A 0x01
#define GPIO_B 0x02
#define GPIO_C 0x03
#define GPIO_D 0x04
#define GPIO_E 0x05

#define BIT0 0
#define BIT1 1
#define BIT2 2
#define BIT3 4
#define BIT4 4
#define BIT5 5
#define BIT6 6
#define BIT7 7
#define BIT8 8
#define BIT9 9
#define BIT10 10
#define BIT11 11
#define BIT12 12
#define BIT13 13
#define BIT14 14
#define BIT15 15

#define HIGH 1
#define LOW 0

typedef enum{
	PD_INPUT = 0,
	PD_OUTPUT
}PD_SIGNAL_DIRECTION;

typedef struct{
	byte port;
	byte pin;
	PD_SIGNAL_DIRECTION direction;
}SIGNAL_ASSIGNMENT;


class Device
{
protected:
	WcaInterfaceHandler *m_pInterface;

private:
	byte m_target_address;

public:
	Device(byte target_address);
	~Device(void);

	bool Initialize(const char* pCOMPort, int baudrate);

	void DeInitialize();

		/** 
	* Execute command with not data to be sent
	* @param destinationAddress	destination address, currently 0x01 is fixed set in library
	* @param prdata				pointer to an already allocated byte buffer of size <size>
	* @param size				size of allocated byte buffer
	* @param written			number of bytes that has been written into the byte buffer 
	* @return					result of execution, true, succesful, otherwise false
	*/
	WcaInterfaceCommandResult SendCommandSync(byte command, byte** prdata, int size, int *written);

	/** 
	* Execute command with data to be sent
	* @param destinationAddress	destination address, currently 0x01 is fixed set in library
	* @param data				byte buffer of data to be sent
	* @param length				length of data to be sent
	* @param prdata				pointer to an already allocated byte buffer of size <size>
	* @param size				size of allocated byte buffer
	* @param written			number of bytes that has been written into the byte buffer 
	* @return					result of execution, true, succesful, otherwise false
	*/
	WcaInterfaceCommandResult SendCommandSync_2(byte command, byte data[], int length, byte** prdata, int size, int *written);

	WcaInterfaceCommandResult SetOutput(byte port, byte pin, byte value);

	WcaInterfaceCommandResult ReadInput(byte port, byte pin, byte *value);

};
