Sub Stock_Analysis()

    Dim ws As Worksheet
    Dim total As Double
    Dim i As Long
    Dim change As Double
    Dim j As Integer
    Dim start As Long
    Dim rowCount As Long
    Dim percentChange As Double
    Dim days As Integer
    Dim dailyChange As Double
    Dim averageChange As Double
    
    ' Loop through each worksheet
    For Each ws In ThisWorkbook.Worksheets
        ' Ensure the analysis starts fresh for each worksheet
        ws.Activate
        
        ' Define headers for each sheet
        ws.Range("I1").Value = "Ticker"
        ws.Range("J1").Value = "Quarterly Change"
        ws.Range("K1").Value = "Percent Change"
        ws.Range("L1").Value = "Total Stock Volume"
        ws.Range("P1").Value = "Ticker"
        ws.Range("Q1").Value = "Value"
        ws.Range("O2").Value = "Greatest % Increase"
        ws.Range("O3").Value = "Greatest % Decrease"
        ws.Range("O4").Value = "Greatest Total Volume"
        
        ' Initialize variables
        j = 0
        total = 0
        change = 0
        start = 2
        
        rowCount = ws.Cells(ws.Rows.Count, "A").End(xlUp).Row
        
        ' Loop through all rows in the current worksheet starting from row 2
        For i = 2 To rowCount
        
            If ws.Cells(i + 1, 1).Value <> ws.Cells(i, 1).Value Then
            
                ' Add up total volume for current ticker
                total = total + ws.Cells(i, 7).Value
                
                If total = 0 Then
                
                    ' No volume data for the ticker, set values to 0
                    ws.Range("I" & 2 + j).Value = ws.Cells(i, 1).Value
                    ws.Range("J" & 2 + j).Value = 0
                    ws.Range("K" & 2 + j).Value = "0%"
                    ws.Range("L" & 2 + j).Value = 0
                    
                Else
                    ' Handle finding the starting price for calculating change
                    If ws.Cells(start, 3).Value = 0 Then
                        For Find_value = start To i
                            If ws.Cells(Find_value, 3).Value <> 0 Then
                                start = Find_value
                                Exit For
                            End If
                        Next Find_value
                    End If
                    
                    ' Calculate change and percent change
                    change = (ws.Cells(i, 6).Value - ws.Cells(start, 3).Value)
                    percentChange = change / ws.Cells(start, 3).Value
                    
                    start = i + 1
                    
                    ' Set calculated values for ticker
                    ws.Range("I" & 2 + j).Value = ws.Cells(i, 1).Value
                    ws.Range("J" & 2 + j).Value = change
                    ws.Range("J" & 2 + j).NumberFormat = "0.00"
                    ws.Range("K" & 2 + j).Value = percentChange
                    ws.Range("K" & 2 + j).NumberFormat = "0.00%"
                    ws.Range("L" & 2 + j).Value = total
                    
                    ' Highlight positive/negative/neutral change
                    Select Case change
                        Case Is > 0
                            ws.Range("J" & 2 + j).Interior.ColorIndex = 4
                        Case Is < 0
                            ws.Range("J" & 2 + j).Interior.ColorIndex = 3
                        Case Else
                            ws.Range("J" & 2 + j).Interior.ColorIndex = 0
                    End Select
                    
                End If
                
                ' Reset values for the next ticker
                total = 0
                change = 0
                j = j + 1
                days = 0
                
            Else
                ' Accumulate total volume for the current ticker
                total = total + ws.Cells(i, 7).Value
            
            End If
        
        Next i
        
        ' Calculate the greatest % increase, decrease, and total volume for each sheet
        ws.Range("Q2") = "%" & WorksheetFunction.Max(ws.Range("K2:K" & rowCount)) * 100
        ws.Range("Q3") = "%" & WorksheetFunction.Min(ws.Range("K2:K" & rowCount)) * 100
        ws.Range("Q4") = WorksheetFunction.Max(ws.Range("L2:L" & rowCount))
        
        ' Find the row with the maximum values and assign to respective cells
        increase_number = WorksheetFunction.Match(WorksheetFunction.Max(ws.Range("K2:K" & rowCount)), ws.Range("K2:K" & rowCount), 0)
        decrease_number = WorksheetFunction.Match(WorksheetFunction.Min(ws.Range("K2:K" & rowCount)), ws.Range("K2:K" & rowCount), 0)
        volume_number = WorksheetFunction.Match(WorksheetFunction.Max(ws.Range("L2:L" & rowCount)), ws.Range("L2:L" & rowCount), 0)
        
        ' Assign tickers corresponding to the max values
        ws.Range("P2") = ws.Cells(increase_number + 1, 9)
        ws.Range("P3") = ws.Cells(decrease_number + 1, 9)
        ws.Range("P4") = ws.Cells(volume_number + 1, 9)
        
    Next ws ' Move to the next worksheet

End Sub

