Public Class Form1
    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        ShowReadMe()
    End Sub

    Private Function GetAbsPathForInputExtract() As String

        Dim fd As OpenFileDialog = New OpenFileDialog()
        Dim strFileName As String = ""

        fd.Title = "Open Extract File"
        fd.InitialDirectory = "C:\"
        fd.Filter = "All files (*.*)|*.*|All files (*.*)|*.*"
        fd.FilterIndex = 2
        fd.RestoreDirectory = True

        If fd.ShowDialog() = DialogResult.OK Then
            strFileName = fd.FileName
        End If

        Return strFileName

    End Function

    Private Function GetAbsPathForKillFile() As String

        Dim fd As OpenFileDialog = New OpenFileDialog()
        Dim strFileName As String = ""

        fd.Title = "Open Kill File"
        fd.InitialDirectory = "C:\"
        fd.Filter = "All files (*.*)|*.*|All files (*.*)|*.*"
        fd.FilterIndex = 2
        fd.RestoreDirectory = True

        If fd.ShowDialog() = DialogResult.OK Then
            strFileName = fd.FileName
        End If

        Return strFileName

    End Function

    Private Function ParseExtractFile(ByRef sFile As String) As Hashtable
        Try
            Dim sSeperator As String = vbCrLf
            Dim sParsed() As String = sFile.Split(sSeperator)
            Dim HT As New Hashtable

            ' For i As Integer = sParsed.GetUpperBound(0) To sParsed.GetLowerBound(0) Step -1
            For i As Integer = sParsed.GetLowerBound(0) To sParsed.GetUpperBound(0)
                If sParsed(i).Length > 0 Then

                    Dim el As New clsExtractedLine
                    el.sLine = sParsed(i)
                    HT.Add(i, el)

                End If
            Next

            Return HT

        Catch ex As Exception
            MsgBox(ex.ToString)
        End Try
    End Function

    Private Function ParseKillFile(ByRef sKillFile As String) As Hashtable
        Try

            Dim sSeperator As String = vbCrLf
            Dim sParsed() As String = sKillFile.Split(sSeperator)
            Dim HT As New Hashtable

            For i As Integer = sParsed.GetLowerBound(0) To sParsed.GetUpperBound(0)
                If sParsed(i).Length > 0 Then
                    HT.Add(sParsed(i), sParsed(i))
                End If
            Next

            Return HT

        Catch ex As Exception
            MsgBox(ex.ToString)
        End Try
    End Function

    Private Sub ShowReadMe()
        Dim s As String = "Kill.txt file should have one line per keyword" & vbCrLf
        s += "Example:" & vbCrLf
        s += "|KillMe|" & vbCrLf
        s += "|RemoveMe|" & vbCrLf
        s += "|OffIGo|"
        MsgBox(s, MsgBoxStyle.OkOnly, "Directions")
    End Sub

    Private Class clsExtractedLine
        Friend sLine As String
        Friend bDelete As Boolean = False
    End Class

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Try



            Dim AbsPathToExtract As String = GetAbsPathForInputExtract()
            If AbsPathToExtract.Length = 0 Then Exit Sub
            Dim sExtract As String = My.Computer.FileSystem.ReadAllText(AbsPathToExtract)

            Dim AbsPathToKillFile As String = GetAbsPathForKillFile()
            If AbsPathToKillFile.Length = 0 Then Exit Sub
            Dim sKillFile As String = My.Computer.FileSystem.ReadAllText(AbsPathToKillFile)

            Dim KillHT As Hashtable = ParseKillFile(sKillFile)
            Dim LinesHT As Hashtable = ParseExtractFile(sExtract)


            For Each Linede As DictionaryEntry In LinesHT
                Dim LineClass As clsExtractedLine = Linede.Value
                For Each Killde As DictionaryEntry In KillHT
                    If InStr(LineClass.sLine, Killde.Key) Then
                        LineClass.bDelete = True
                    End If
                Next
            Next




            Dim SBACKWARD As New List(Of String)()
            For Each Linede As DictionaryEntry In LinesHT
                Dim LineClass As clsExtractedLine = Linede.Value
                If LineClass.bDelete = False Then
                    SBACKWARD.Add(LineClass.sLine & vbCrLf)
                End If
            Next

            Dim myArray As String() = SBACKWARD.ToArray()
            Dim sb As New System.Text.StringBuilder()
            For I As Integer = myArray.GetUpperBound(0) To myArray.GetLowerBound(0) Step -1
                sb.Append(myArray(I).ToString)
            Next


            'Dim sb As New System.Text.StringBuilder()
            'For Each Linede As DictionaryEntry In LinesHT
            '    Dim LineClass As clsExtractedLine = Linede.Value
            '    If LineClass.bDelete = False Then
            '        sb.Append(LineClass.sLine & vbCrLf)
            '    End If
            'Next
            'MsgBox(sb.ToString)
            My.Computer.FileSystem.WriteAllText(AbsPathToExtract & ".out.txt", sb.ToString, False)

            MsgBox("DONE", MsgBoxStyle.OkOnly)

        Catch ex As Exception
            MsgBox(ex.ToString)
        End Try
    End Sub
End Class
