Imports System.Data.SQLite
Imports System.Data.SqlClient
Imports System.Globalization



Public Class dbOperation_keepa

    Public Shared DBconnStr As String = "Data Source=|DataDirectory|\keepadb.db; Version=3;"

    Public Shared Function GetDataTable(ByVal Qry As [String], ByRef _status As Integer) As DataTable
        Dim dt As DataTable = Nothing
        Try
            _status = 1
            Using con As New SQLiteConnection(DBconnStr)
                con.Open()
                Dim cmd As New SQLiteCommand(Qry, con)
                cmd.CommandText = Qry

                Dim da As New SQLiteDataAdapter(cmd)
                Dim ds As New DataSet()
                da.Fill(ds, "data")
                con.Close()
                dt = ds.Tables("data")
                Return dt
            End Using
        Catch
            _status = -1
            Return dt
        End Try
    End Function

    Public Shared Function GetDataTable(ByVal cmd As SQLiteCommand, ByRef _status As Integer) As DataTable
        Dim dt As DataTable = Nothing
        Try
            _status = 1
            Using con As New SQLiteConnection(DBconnStr)
                con.Open()
                cmd.Connection = con

                Dim da As New SQLiteDataAdapter(cmd)
                Dim ds As New DataSet()
                da.Fill(ds, "data")
                con.Close()
                dt = ds.Tables("data")
                Return dt
            End Using
        Catch
            _status = -1
            Return dt
        End Try
    End Function

    Public Shared Function ExecuteQuery(cmd As SQLiteCommand) As Int32
        Try
            Dim i As Integer = 0
            Dim con As New SQLiteConnection(DBconnStr)
            cmd.Connection = con
            cmd.CommandType = CommandType.Text
            con.Open()
            i = cmd.ExecuteNonQuery()
            cmd.Parameters.Clear()
            con.Close()
            Return i
        Catch
            Return -1
        End Try
    End Function

    Public Shared Function ExecuteQuery(qry As [String]) As Int32
        Try
            Dim i As Integer = 0
            Dim con As New SQLiteConnection(DBconnStr)
            Dim cmd As New SQLiteCommand(qry)
            cmd.Connection = con
            cmd.CommandType = CommandType.Text
            con.Open()
            Using transaction = con.BeginTransaction()
                i = cmd.ExecuteNonQuery()
                transaction.Commit()
            End Using

            cmd.Parameters.Clear()
            con.Close()
            Return i
        Catch
            Return -1
        End Try
    End Function

    Public Shared Function RemoveExtraFrokKeepaDB(ByVal dt As DataTable) As Int32
        Try
            Dim qry As String = "delete from keepa_price where product_asin not in (" & "" & ")"
            Dim i As Integer = 0
            Dim con As New SQLiteConnection(DBconnStr)
            Dim cmd As New SQLiteCommand(qry)
            cmd.Connection = con
            cmd.CommandType = CommandType.Text
            con.Open()
            Using transaction = con.BeginTransaction()
                i = cmd.ExecuteNonQuery()
                transaction.Commit()
            End Using

            cmd.Parameters.Clear()
            con.Close()
            Return i
        Catch
            Return -1
        End Try
    End Function
End Class
