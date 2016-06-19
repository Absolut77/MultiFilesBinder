Imports System.Text

Public Class Form1

    Private Sub ToolStripMenuItem1_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItem1.Click
        Using OFD As New OpenFileDialog
            OFD.Filter = "All Files *|*"
            If OFD.ShowDialog = vbOK Then
                For i = 0 To ListView1.Items.Count - 1
                    If ListView1.Items(i).SubItems(0).Text = OFD.SafeFileName Then
                        MsgBox("Error you can't add a file who have the same name.", MsgBoxStyle.Critical)
                        Exit Sub
                    End If
                Next
                Dim L As New ListViewItem(OFD.SafeFileName)
                L.SubItems.Add(OFD.FileName)
                L.SubItems.Add(O.GetFileSizeKB(OFD.FileName))
                L.SubItems.Add(IO.Path.GetExtension(OFD.FileName))
                ListView1.Items.Add(L)
            End If
        End Using
    End Sub

    Private Sub ToolStripMenuItem2_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItem2.Click
        For Each ele As ListViewItem In ListView1.SelectedItems
            ele.Remove()
        Next
    End Sub

    Private Sub ToolStripMenuItem3_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItem3.Click
        Dim Resource_P As String = Application.StartupPath & "\B.resources"
        Dim Core_P As String = Application.StartupPath & "\Core.dll"
        Using SFD As New SaveFileDialog
            SFD.Filter = "Portable Executable |*.exe"
            If SFD.ShowDialog = vbOK Then
                Using R As New Resources.ResourceWriter(Resource_P)
                    For i = 0 To ListView1.Items.Count - 1
                        Dim data As Byte() = O.Compress(IO.File.ReadAllBytes(ListView1.Items(i).SubItems(1).Text))
                        Dim rName As String = String.Format(ListView1.Items(i).SubItems(0).Text)
                        R.AddResource(rName, data)
                    Next
                    R.Generate()
                End Using
                Codedom.compile_Core(My.Resources.Core, Core_P, Resource_P, True)
                IO.File.Delete(Resource_P)
                Dim S As String = My.Resources.Stub
                Dim result As MsgBoxResult = MsgBox("Do you want to use a custom icon ?", MsgBoxStyle.YesNo, "Question")
                Dim key_String As String = O.Random
                S = S.Replace("%KeyString%", key_String)

                Using R As New Resources.ResourceWriter(Resource_P)
                    R.AddResource(Encoding.Default.GetString(O.Proper_RC4(IO.File.ReadAllBytes(Core_P), Encoding.Default.GetBytes(key_String))), "f")
                    R.Generate()
                End Using
                If result = MsgBoxResult.Yes Then
                    Using O As New OpenFileDialog
                        O.Filter = "Icon File |*.ico"
                        If O.ShowDialog = vbOK Then
                            Codedom.compile_Stub(S, SFD.FileName, Resource_P, True, O.FileName)
                        Else
                            Codedom.compile_Stub(S, SFD.FileName, Resource_P, True, Nothing)
                        End If
                    End Using
                Else
                    Codedom.compile_Stub(S, SFD.FileName, Resource_P, True, Nothing)
                End If

                IO.File.Delete(Resource_P)
                IO.File.Delete(Core_P)
                MsgBox("Sucess !", MsgBoxStyle.Information)
            End If
        End Using
    End Sub
End Class
