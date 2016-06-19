Imports System.IO
Imports System.IO.Compression

Public Class O

    Public Shared Function Random() As String
        Dim alphabet As String = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ"
        Dim r As New Random
        Dim password As String = New String((alphabet.OrderBy(Function(n) Guid.NewGuid).Take(r.Next(4, 20))).ToArray())
        Return password
    End Function

    Public Shared Function GetFileSizeKB(ByVal path As String) As String
        Return IO.File.ReadAllBytes(path).Length / 1024 & " Kb"
    End Function

    Public Shared Function Compress(data As Byte()) As Byte()
        Dim output As New MemoryStream()
        Dim gzip As New GZipStream(output, CompressionMode.Compress, True)
        gzip.Write(data, 0, data.Length)
        gzip.Close()
        Return output.ToArray()
    End Function

    Public Shared Function Decompress(data As Byte()) As Byte()
        Dim input As New MemoryStream()
        input.Write(data, 0, data.Length)
        input.Position = 0
        Dim gzip As New GZipStream(input, CompressionMode.Decompress, True)
        Dim output As New MemoryStream()
        Dim buff As Byte() = New Byte(63) {}
        Dim read As Integer = -1
        read = gzip.Read(buff, 0, buff.Length)
        While read > 0
            output.Write(buff, 0, read)
            read = gzip.Read(buff, 0, buff.Length)
        End While
        gzip.Close()
        Return output.ToArray()
    End Function

    Public Shared Function Proper_RC4(ByVal Input As Byte(), ByVal Key As Byte()) As Byte()
        'Leave a thanks at least..
        'by d3c0mpil3r from HF
        Dim i, j, swap As UInteger
        Dim s As UInteger() = New UInteger(255) {}
        Dim Output As Byte() = New Byte(Input.Length - 1) {}

        For i = 0 To 255
            s(i) = i
        Next

        For i = 0 To 255
            j = (j + Key(i Mod Key.Length) + s(i)) And 255
            swap = s(i) 'Swapping of s(i) and s(j)
            s(i) = s(j)
            s(j) = swap
        Next

        i = 0 : j = 0
        For c = 0 To Output.Length - 1
            i = (i + 1) And 255
            j = (j + s(i)) And 255
            swap = s(i) 'Swapping of s(i) and s(j)
            s(i) = s(j)
            s(j) = swap
            Output(c) = Input(c) Xor s((s(i) + s(j)) And 255)
        Next

        Return Output
    End Function
End Class
