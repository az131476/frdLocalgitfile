
// TestHardWare.h : PROJECT_NAME Ӧ�ó������ͷ�ļ�
//

#pragma once

#ifndef __AFXWIN_H__
	#error "�ڰ������ļ�֮ǰ������stdafx.h�������� PCH �ļ�"
#endif

#include "resource.h"		// ������


// CTestHardWareApp:
// �йش����ʵ�֣������ TestHardWare.cpp
//

class CTestHardWareApp : public CWinApp
{
public:
	CTestHardWareApp();

// ��д
public:
	virtual BOOL InitInstance();

// ʵ��

	DECLARE_MESSAGE_MAP()
};

extern CTestHardWareApp theApp;