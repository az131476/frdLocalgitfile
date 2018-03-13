// DlgShowData.cpp : 实现文件
//

#include "stdafx.h"
#include "TestHardWare.h"
#include "DlgShowData.h"
#include "afxdialogex.h"
#include "Common.h"

// CDlgShowData 对话框

IMPLEMENT_DYNAMIC(CDlgShowData, CDialogEx)

CDlgShowData::CDlgShowData(CWnd* pParent /*=NULL*/)
	: CDialogEx(CDlgShowData::IDD, pParent)
{

}

CDlgShowData::~CDlgShowData()
{
}

void CDlgShowData::DoDataExchange(CDataExchange* pDX)
{
	CDialogEx::DoDataExchange(pDX);
	DDX_Control(pDX, IDC_LIST_SHOWDATA, m_List);
}


BEGIN_MESSAGE_MAP(CDlgShowData, CDialogEx)
	ON_WM_DESTROY()
	ON_MESSAGE(SHOW_SAMPLE_DATA, &CDlgShowData::OnShowSampleData)
END_MESSAGE_MAP()


// CDlgShowData 消息处理程序


BOOL CDlgShowData::PreTranslateMessage(MSG* pMsg)
{
	// TODO: 在此添加专用代码和/或调用基类
	if (pMsg->wParam == VK_ESCAPE || pMsg->wParam == VK_RETURN)
		return TRUE;

	return CDialogEx::PreTranslateMessage(pMsg);
}


void CDlgShowData::OnDestroy()
{
	this->GetParent()->SendMessage(CLOSE_SHOWDATA_WINDOW);
//	CDialogEx::OnDestroy();
}


afx_msg LRESULT CDlgShowData::OnShowSampleData(WPARAM wParam, LPARAM lParam)
{
// 	int nDataCount = (int)wParam;
// 	float *pfltData = (float *)lParam;
// 	CString strText = "", strData = "";
// 	for (int i = 0; i < nDataCount; i++)
// 	{
// 		if (i % 15 == 0)
// 			strText += "\r\n";
// 		strData.Format("%.3f   ", pfltData[i]);
// 		strText += strData;
// 	}
// 	strText += "\r\n";
// 	m_Edit.SetWindowTextA(strText);
// 	m_Edit.SetSel(m_Edit.GetWindowTextLength(), m_Edit.GetWindowTextLength());
	int nDataCount = (int)wParam;
	float *pfltData = (float *)lParam;
	CString strText = "", strData = "";
	for (int i = 0; i < nDataCount; i++)
	{
		if (i % 30 == 0)
		{
			m_List.AddString(strText);
			strText = "";
		}
		strData.Format("%.3f   ", pfltData[i]);
		strText += strData;
	}
	m_List.AddString(strText);
	int nCount = m_List.GetCount();
	m_List.SetCurSel(nCount-1);
	return 0;
}

BOOL CDlgShowData::OnInitDialog()
{
	CDialogEx::OnInitDialog();

	::SendMessageA(m_List, LB_SETHORIZONTALEXTENT, 2000, 0);

	return TRUE;  // return TRUE unless you set the focus to a control
	// 异常: OCX 属性页应返回 FALSE
}
