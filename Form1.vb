Imports System.ComponentModel
Imports System.IO
Imports System.Windows.Forms.DataVisualization.Charting
Public Class Form1
    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Text = "COVID-19 CASE MAPPER"
        covidchart.Hide()
        If File.Exists("data.txt") Then
            My.Computer.FileSystem.DeleteFile("data.txt")
        End If
        My.Computer.Network.DownloadFile("https://covid19.who.int/WHO-COVID-19-global-data.csv", "data.txt")
        If File.Exists("listcountriesterritories.txt") Then
            My.Computer.FileSystem.DeleteFile("listcountriesterritories.txt")
            Dim listcountriesterritoriesfile As New FileStream("listcountriesterritories.txt", FileMode.CreateNew)
            listcountriesterritoriesfile.Close()
        Else
            Dim listcountriesterritoriesfile As New FileStream("listcountriesterritories.txt", FileMode.CreateNew)
            listcountriesterritoriesfile.Close()
        End If
        getcountriesterritories()
        setupcountriesterritoriescombobox()
        setupdeathscasescumulativedailycombobox()
    End Sub
    Sub getcountriesterritories()
        Dim line As String()
        Dim line2 As String
        Dim countryterritory As String
        Dim inlist As Boolean
        Dim retry As Boolean = True
        Using reader As New StreamReader("data.txt")
            reader.ReadLine()
            Do
                line = reader.ReadLine.Split(New Char() {","c})
                countryterritory = line(2)
                Using reader2 As New StreamReader("listcountriesterritories.txt")
                    Do
                        line2 = reader2.ReadLine
                        If countryterritory = line2 Then
                            inlist = True
                        End If
                    Loop Until reader2.EndOfStream
                End Using
                While retry
                    Try
                        If inlist = False Then
                            Using writer As New StreamWriter("listcountriesterritories.txt", True)
                                writer.WriteLine(countryterritory)
                            End Using
                        End If
                        inlist = False
                        retry = False
                    Catch ex As IOException
                    End Try
                End While
                retry = True
            Loop Until reader.EndOfStream
        End Using
    End Sub
    Sub setupcountriesterritoriescombobox()
        Using reader As New StreamReader("listcountriesterritories.txt")
            Do
                countryterritorycombobox.Items.Add(reader.ReadLine())
            Loop Until reader.EndOfStream
        End Using

    End Sub
    Sub setupdeathscasescumulativedailycombobox()
        deathscasescombobox.Items.Add("Deaths")
        deathscasescombobox.Items.Add("Cases")
        cumulativedailycombobox.Items.Add("Cumulative")
        cumulativedailycombobox.Items.Add("Daily")
    End Sub

    Private Sub buttongenerategraph_Click(sender As Object, e As EventArgs) Handles buttongenerategraph.Click
        Dim country As String = countryterritorycombobox.Text
        Dim deathcase As String = deathscasescombobox.Text
        Dim cumulativedaily As String = cumulativedailycombobox.Text
        If country = "" Or deathcase = "" Or cumulativedaily = "" Then
            MsgBox("Please enter values")
        Else
            covidchart.Show()
            covidchart.Titles.Clear()
            covidchart.Titles.Add(country & "'s " & deathcase)
            covidchart.Series.Clear()
            covidchart.ChartAreas.Clear()
            covidchart.ChartAreas.Add("chartarea")
            covidchart.Series.Clear()
            covidchart.Series.Add("data")
            covidchart.Series(0).IsVisibleInLegend = False
            Dim line As String()
            Using reader As New StreamReader("data.txt")
                If deathcase = "Deaths" And cumulativedaily = "Cumulative" Then
                    line = reader.ReadLine.Split(New Char() {","c})
                    Do
                        line = reader.ReadLine.Split(New Char() {","c})
                        If line(2) = country Then
                            covidchart.Series(0).Points.AddXY(line(0), line(7))
                        End If
                    Loop Until reader.EndOfStream
                ElseIf deathcase = "Deaths" And cumulativedaily = "Daily" Then
                    line = reader.ReadLine.Split(New Char() {","c})
                    Do
                        line = reader.ReadLine.Split(New Char() {","c})
                        If line(2) = country Then
                            covidchart.Series(0).Points.AddXY(line(0), line(6))
                        End If
                    Loop Until reader.EndOfStream
                ElseIf deathcase = "Cases" And cumulativedaily = "Cumulative" Then
                    line = reader.ReadLine.Split(New Char() {","c})
                    Do
                        line = reader.ReadLine.Split(New Char() {","c})
                        If line(2) = country Then
                            covidchart.Series(0).Points.AddXY(line(0), line(5))
                        End If
                    Loop Until reader.EndOfStream
                ElseIf deathcase = "Cases" And cumulativedaily = "Daily" Then
                    line = reader.ReadLine.Split(New Char() {","c})
                    Do
                        line = reader.ReadLine.Split(New Char() {","c})
                        If line(2) = country Then
                            covidchart.Series(0).Points.AddXY(line(0), line(4))
                        End If
                    Loop Until reader.EndOfStream
                End If
            End Using
        End If

    End Sub

    Private Sub Form1_FormClosing(sender As Object, e As FormClosingEventArgs) Handles Me.FormClosing
        My.Computer.FileSystem.DeleteFile("data.txt")
        My.Computer.FileSystem.DeleteFile("listcountriesterritories.txt")
    End Sub
End Class