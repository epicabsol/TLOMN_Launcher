Public Class Form1

    Public Const EXEName As String = "LEGO Bionicle.exe"
    Public Const InstallRegistryKey As String = "SOFTWARE\WOW6432Node\Microsoft\Windows\CurrentVersion\Uninstall\LEGO Bionicle" ' HKLM

    ''' <summary>
    ''' Determines the TLOMN install folder first by reading the configuration,
    ''' then by searching for the standard registry key, then by searching the current directory,
    ''' and finally by asking the user.
    ''' </summary>
    ''' <returns>The full path of the folder containing the game executable.</returns>
    Private Function DetectInstallFolder() As String
        Dim result As String = ""

        Dim key As Microsoft.Win32.RegistryKey = My.Computer.Registry.LocalMachine.OpenSubKey(InstallRegistryKey)

        If Not My.Settings.InstallFolder = "" Then
            ' The setting wasn't empty, use it!
            result = My.Settings.InstallFolder
        ElseIf key IsNot Nothing Then
            ' The installation registry key exists, use it!
            result = IO.Path.GetDirectoryName(key.GetValue("DisplayIcon").ToString().Split(",")(0))
        ElseIf IO.File.Exists(IO.Path.Combine(My.Application.Info.DirectoryPath, EXEName)) Then
            ' The game is in the same folder as this launcher!
            result = My.Application.Info.DirectoryPath
        Else
            ' We couldn't find the game, ask the user.
            Dim browser As New FolderBrowserDialog()
            browser.Description = "Choose the game folder (Installed or not)"
            If browser.ShowDialog() = DialogResult.OK Then
                result = browser.SelectedPath
            Else
                ' The user canceled, so store "" as the folder (so we go through this process again next launch), and then exit the program.
                Application.Exit() ' Usually not good practice, here I don't care.
            End If
        End If

        key?.Dispose()

        ' Save the value we found into the setting for next time
        My.Settings.InstallFolder = result
        My.Settings.Save()

        Return result
    End Function

    Private Sub LaunchGame()
        Process.Start(IO.Path.Combine(DetectInstallFolder(), EXEName))
    End Sub

    Private Function IsGameDownloaded() As Boolean
        Dim folder As String = DetectInstallFolder()
        Return IO.File.Exists(IO.Path.Combine(folder, EXEName))
    End Function

    Private Function GetServerVersion() As String
        Dim client As Net.WebClient = New Net.WebClient()
        Dim result As String = client.DownloadString("http://biomediaproject.com/bmp/files/gms/tlomn/Launcher/Patch/version.txt")
        client.Dispose()
        Return result
    End Function

    Private Function GetLocalVersion() As String
        Return IO.File.ReadAllText("version.txt")
    End Function

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Dim GameFolder As String = DetectInstallFolder()

        If IsGameDownloaded() Then
            Dim serverVersion As String = GetServerVersion()
            Dim localVersion As String = GetLocalVersion()
            If serverVersion <> localVersion Then
                MessageBox.Show("TLOMN Launcher", "New patch version: " & serverVersion & " (update from " & localVersion & ")",
                                MessageBoxButtons.OKCancel, MessageBoxIcon.Hand)
                My.Computer.Network.DownloadFile(
               "http://biomediaproject.com/bmp/files/gms/tlomn/Launcher/Patch/Beaverhouse%20TLOMN%20Patch.exe",
               "Temp\Beaverhouse TLOMN Patch.exe", "", "", True, 1000, True)
                Process.Start("Temp\Beaverhouse TLOMN Patch.exe").WaitForExit()
            End If
            My.Computer.FileSystem.DeleteDirectory("Temp", FileIO.DeleteDirectoryOption.DeleteAllContents)
            LaunchGame()
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
            LaunchGame()
        End If
        Close()
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        Process.Start("Debug Menu Fix.exe")
    End Sub
End Class
