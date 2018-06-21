#pragma once

#include "stdafx.h"
#define _ATL_ATTRIBUTES
#include <atlbase.h>
#include <atlcom.h>
#include <comutil.h>


#ifdef _DEBUG
#import "C:\projects\0_31X_S0500_moray\trunk\development\software\app\source\pc_tools\WChBootloaderHandlerV0\WCAInterface\bin\Debug\/WCAInterfaceBootloader.tlb" no_namespace, named_guids, raw_interfaces_only
#else
#import "../WCAInterface/bin/Release/WCAInterfaceBootloader.tlb" no_namespace named_guids
#endif
 
class WCAInterfaceBootloaderHandler
{
private:
	IWCAInterfaceBootloader *m_pWCAInterfaceBootloader;
	bool m_IsInitialized;

public:
	WCAInterfaceBootloaderHandler(void);
	
public:
	~WCAInterfaceBootloaderHandler(void);

	bool Initialize(char* pCOMPort);
	
	void DeInitialize();

	bool Connect();

	char* ReadBootloaderParameter();

	char* ReadApplicationParameter();

	bool StartApplication(unsigned short start_param);

};
