#pragma once

#include "stdafx.h"
#define _ATL_ATTRIBUTES
#include <atlbase.h>
#include <atlcom.h>
#include <comutil.h>
//#include "comvector.h"

#include <sstream>

#define SSTR( x ) static_cast< std::ostringstream & >( \
        ( std::ostringstream() << std::dec << x ) ).str()


#ifdef _DEBUG
#import "../WcaInterfaceLibrary/bin/Debug/WcaInterfaceLibrary.tlb" no_namespace, named_guids, raw_interfaces_only
#else
#import "../WcaInterfaceLibrary/bin/Release/WcaInterfaceLibrary.tlb" no_namespace, named_guids, raw_interfaces_only
#endif


class WcaInterfaceHandler
{
private:
	IWcaInterface *m_pWcaInterface;
	bool m_IsInitialized;

public:
	WcaInterfaceHandler(void);
	
public:
	~WcaInterfaceHandler(void);

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
	WcaInterfaceCommandResult SendCommandSync(byte destinationAddress, byte command, byte** prdata, int size, int *written);

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
	WcaInterfaceCommandResult SendCommandSync_2(byte destinationAddress, byte command, byte data[], int length, byte** prdata, int size, int *written);

	std::string GetVersion(void);
};
