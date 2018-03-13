#pragma once
#include "afxwin.h"

// CDlgShowData 对话框

class CDlgShowData : public CDialogEx
{
	DECLARE_DYNAMIC(CDlgShowData)

public:
	CDlgShowData(CWnd* pParent = NULL);   // 标准构造函数
	virtual ~CDlgShowData();

// 对话框数据
	enum { IDD = IDD_DLG_SHOWDATA };

protected:
	virtual void DoDataExchange(CDataExchange* pDX);    // DDX/DDV 支持

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
