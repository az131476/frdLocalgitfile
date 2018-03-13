#include "stdafx.h"
#include "Common.h"

void GetValidFloatString(string strText, string &strFloat)
{
	CString str = strText.data();
	while (strcmp(str.Right(1), "0") == 0)
	{
		str = str.Left(str.GetLength()-1);
	}
	if (strcmp(str.Right(1), ".") == 0)
	{
		str = str.Left(str.GetLength()-1);
	}
	strFloat = str;
}