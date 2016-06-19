Imports System.Runtime.InteropServices

Public Class ListView
    Inherits System.Windows.Forms.ListView

    <DllImport("uxtheme.dll", CharSet:=CharSet.Unicode)> _
    Private Shared Function SetWindowTheme(ByVal hWnd As IntPtr, ByVal pszSubAppName As String, ByVal pszSubIdList As String) As Integer
    End Function
    Protected Overrides Sub CreateHandle()
        MyBase.CreateHandle()
        SetWindowTheme(Me.Handle, "explorer", Nothing)
    End Sub
    Public Sub New()
        Me.DoubleBuffered = True
        Me.View = System.Windows.Forms.View.Details
        Me.FullRowSelect = True
        Me.GridLines = True
    End Sub
End Class