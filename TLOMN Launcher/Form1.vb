Public Class Form1
    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        If My.Computer.FileSystem.FileExists("LEGO Bionicle.exe") Then
            My.Computer.Network.DownloadFile(
            "http://biomediaproject.com/bmp/files/gms/tlomn/Launcher/Patch/version.txt",
            "Temp\version.txt", "", "", True, 1000, True)
            Dim text1 As String = My.Computer.FileSystem.ReadAllText("Temp\version.txt")
            Dim text2 As String = My.Computer.FileSystem.ReadAllText("version.txt")
            If text1 <> text2 Then
                MessageBox.Show("put your text here", "here the title of you message",
                                MessageBoxButtons.OKCancel, MessageBoxIcon.Hand)
                My.Computer.Network.DownloadFile(
               "http://biomediaproject.com/bmp/files/gms/tlomn/Launcher/Patch/Beaverhouse%20TLOMN%20Patch.exe",
               "Temp\Beaverhouse TLOMN Patch.exe", "", "", True, 1000, True)
                Process.Start("Temp\Beaverhouse TLOMN Patch.exe").WaitForExit()
            End If
            My.Computer.FileSystem.DeleteDirectory("Temp", FileIO.DeleteDirectoryOption.DeleteAllContents)
            Process.Start("LEGO Bionicle.exe")
        Else
            My.Computer.Network.DownloadFile(
            "http://biomediaproject.com/bmp/files/gms/tlomn/Launcher/setup.exe",
            "Temp\setup.exe", "", "", True, 1000, True)
            Process.Start("Temp\TLOMN\setup.exe").WaitForExit()
            My.Computer.Network.DownloadFile(
                       "http://biomediaproject.com/bmp/files/gms/tlomn/Launcher/Patch/Beaverhouse%20TLOMN%20Patch.exe",
                       "Temp\Beaverhouse TLOMN Patch.exe", "", "", True, 1000, True)
            Process.Start("Temp\Beaverhouse TLOMN Patch.exe").WaitForExit()
            My.Computer.FileSystem.DeleteDirectory("Temp", FileIO.DeleteDirectoryOption.DeleteAllContents)
            Process.Start("LEGO Bionicle.exe")
        End If
        MyBase.Close()
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        Process.Start("Debug Menu Fix.exe")
    End Sub
End Class
