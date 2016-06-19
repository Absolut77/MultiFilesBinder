Imports System.CodeDom
Imports System.IO
Imports System.Reflection


Public Class Codedom

    Public Shared Function compile_Stub(ByVal input As String, ByVal output As String, ByVal resources As String, ByVal showError As Boolean, Optional ByVal icon_Path As String = Nothing) As Boolean

        Dim provider_Args As New Dictionary(Of String, String)()
        provider_Args.Add("CompilerVersion", "v4.0")

        Dim provider As New Microsoft.VisualBasic.VBCodeProvider(provider_Args)
        Dim c_Param As New Compiler.CompilerParameters
        Dim c_Args As String = " /target:winexe /platform:x86 /optimize "

        If Not icon_Path = Nothing Then
            c_Args = c_Args & " /win32icon:" & icon_Path
        Else
            c_Args = c_Args & " /win32icon:" & GenIcon(32, 32)
        End If

        c_Param.GenerateExecutable = True
        c_Param.OutputAssembly = output
        c_Param.EmbeddedResources.Add(resources)
        c_Param.CompilerOptions = c_Args
        c_Param.IncludeDebugInformation = False

        c_Param.ReferencedAssemblies.AddRange({"System.Dll", "System.Windows.Forms.Dll", "System.Xml.Linq.dll", "System.Core.dll", "Microsoft.VisualBasic.dll"})


        Dim c_Result As Compiler.CompilerResults = provider.CompileAssemblyFromSource(c_Param, input)
        If c_Result.Errors.Count = 0 Then
            Return True
        Else
            If showError Then
                For Each _Error As Compiler.CompilerError In c_Result.Errors
                    MessageBox.Show(_Error.ToString)
                Next
                Return False
            End If
            Return False
        End If
    End Function

    Public Shared Function compile_Core(ByVal input As String, ByVal output As String, ByVal resources As String, ByVal showError As Boolean) As Boolean

        Dim provider_Args As New Dictionary(Of String, String)()
        provider_Args.Add("CompilerVersion", "v4.0")

        Dim provider As New Microsoft.VisualBasic.VBCodeProvider(provider_Args)
        Dim c_Param As New Compiler.CompilerParameters
        Dim c_Args As String = " /target:library /platform:x86 /optimize /define:_MYTYPE=\""""Empty\"""""

        c_Param.GenerateExecutable = True
        c_Param.OutputAssembly = output
        c_Param.CompilerOptions = c_Args
        c_Param.IncludeDebugInformation = False
        c_Param.EmbeddedResources.Add(resources)
        c_Param.ReferencedAssemblies.AddRange({"System.Dll", "System.Windows.Forms.Dll"})

        Dim comp_Result As Compiler.CompilerResults = provider.CompileAssemblyFromSource(c_Param, input)
        If comp_Result.Errors.Count = 0 Then
            Return True
        Else
            If showError Then
                For Each _Error As Compiler.CompilerError In comp_Result.Errors
                    MessageBox.Show(_Error.ToString)
                Next
                Return False
            End If
            Return False
        End If
    End Function

    Public Shared Function GenIcon(w As Integer, h As Integer) As String
        Dim path As String = IO.Path.GetTempPath & "Randomicon.ico"
        If IO.File.Exists(path) Then
            IO.File.Delete(path)
        End If
        Dim r As New Random
        Using bm As New Bitmap(w, h)
            Dim lb = bm.LockBits(New Rectangle(0, 0, bm.Width, bm.Height), Imaging.ImageLockMode.ReadWrite, bm.PixelFormat)
            Dim byt(lb.Stride * bm.Height - 1) As Byte
            For i = 0 To byt.Length - 1
                byt(i) = r.Next(0, 255)
            Next
            Runtime.InteropServices.Marshal.Copy(byt, 0, lb.Scan0, byt.Length)
            bm.UnlockBits(lb)
            Dim HIcon As IntPtr = bm.GetHicon()
            Dim newIcon As Icon = Icon.FromHandle(HIcon)
            Using f As New IO.FileStream(path, FileMode.CreateNew)
                newIcon.Save(f)
            End Using
        End Using
        Return path
    End Function

End Class
