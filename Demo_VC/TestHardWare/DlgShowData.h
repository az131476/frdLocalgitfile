#pragma once
#include "afxwin.h"

// CDlgShowData �Ի���

class CDlgShowData : public CDialogEx
{
	DECLARE_DYNAMIC(CDlgShowData)

public:
	CDlgShowData(CWnd* pParent = NULL);   // ��׼���캯��
	virtual ~CDlgShowData();

// �Ի�������
	enum { IDD = IDD_DLG_SHOWDATA };

protected:
	virtual void DoDataExchange(CDataExchange* pDX);    // DDX/DDV ֧��

	DECLARE_MESSAGE_MAP()
public:
	virtual BOOL PreTranslateMessage(MSG* pMsg);
	afx_msg void OnDestroy();
protected:
	afx_msg LRESULT OnShowSampleData(WPARAM wParam, LPARAM lParam);
public:
	CListBox m_List;
	virtual BOOL OnInitDialog();
};
