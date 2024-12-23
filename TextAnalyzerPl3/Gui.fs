﻿module Gui


open Analysis
open System
open System.IO
open System.Windows.Forms
open System.Drawing

let mutable loadedText = ""



let createStyledButton (text: string) (width: int) (backColor: Color) =
    let button = new Button(Text = text, Width = width, Height = 50, BackColor = backColor, FlatStyle = FlatStyle.Standard)
    button.Font <- new Font("Arial", 12.0f, FontStyle.Bold)
    button.Padding <- new Padding(15)
    button.ForeColor <- Color.Black 
    button.BackColor <- backColor
    button.FlatAppearance.BorderSize <- 1
    button.FlatAppearance.BorderColor <- Color.Black
    button.TextAlign <- ContentAlignment.MiddleCenter

    button.MouseEnter.Add(fun _ -> button.BackColor <- Color.LightGray)
    button.MouseLeave.Add(fun _ -> button.BackColor <- backColor)

    button.Region <- new Region(new Drawing.RectangleF(0.0f, 0.0f, button.Width |> float32, button.Height |> float32))

    button

let showAnalysisResults text =
    let wordCount = countWords text
    let sentenceCount = countSentences text
    let paragraphCount = countParagraphs text
    let avgSentenceLength = averageSentenceLength text
    let wordFreq = wordFrequency text

    let tableRows = 
        wordFreq
        |> Array.map (fun (word, freq) -> sprintf "word : %s  freq : %d " word freq)
        |> String.concat "\n"

    let analysisResult = sprintf """
| Metric                  | Value    |
|-------------------------|----------|
| Words                   | %d       |
| Sentences               | %d       |
| Paragraphs              | %d       |
| Average Sentence Length | %.2f     |

Word Frequency:
%s
                                """ wordCount sentenceCount paragraphCount avgSentenceLength tableRows

    analysisResult

let createTextAnalyzerApp () =
    let form = new Form(Text = "Text Analyzer", Width = 400, Height = 500, StartPosition = FormStartPosition.CenterScreen)

    
    
    form.FormBorderStyle <- FormBorderStyle.FixedDialog
    form.MaximizeBox <- false
    form.MinimizeBox <- false
    form.BackColor <- Color.White

    
    form.ClientSize <- new Size(370, 280)

    
    let buttonPanel = new Panel(Dock = DockStyle.Top, Height = 350)
    form.Controls.Add(buttonPanel)

    
    let flowLayout = new FlowLayoutPanel(Dock = DockStyle.Fill, FlowDirection = FlowDirection.LeftToRight, Padding = new Padding(25))
    buttonPanel.Controls.Add(flowLayout)

    
    let analyzeButton = createStyledButton "Analyze" 150 Color.Gray
    let loadButton = createStyledButton "Load File" 150 Color.Gray
    let clearButton = createStyledButton "Clear Text" 150 Color.Gray
    let saveButton = createStyledButton "Save Results" 150 Color.Gray
    let showResultsButton = createStyledButton "Show Results" 150 Color.Gray
    let showTextButton = createStyledButton "Show Text" 150 Color.Gray

    
    flowLayout.Controls.Add(analyzeButton)
    flowLayout.Controls.Add(loadButton)
    flowLayout.Controls.Add(clearButton)
    flowLayout.Controls.Add(saveButton)
    flowLayout.Controls.Add(showResultsButton)
    flowLayout.Controls.Add(showTextButton)


    analyzeButton.Click.Add(fun _-> 
        if loadedText <> "" then
            let result = showAnalysisResults loadedText
            MessageBox.Show(result, "Analysis Results", MessageBoxButtons.OK, MessageBoxIcon.Information) |> ignore
        else
            MessageBox.Show("Please load a file first.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning) |> ignore
    )

    loadButton.Click.Add(fun _-> 
        let openFileDialog = new OpenFileDialog(Filter = "Text Files (*.txt)|*.txt")
        if openFileDialog.ShowDialog() = DialogResult.OK then
            let filePath = openFileDialog.FileName
            loadedText <- File.ReadAllText(filePath)
    )

    clearButton.Click.Add(fun _-> 
        loadedText <- ""
        MessageBox.Show("Text cleared.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information) |> ignore
    )


    saveButton.Click.Add(fun _-> 
        if loadedText <> "" then
            let analysisResult = showAnalysisResults loadedText
            if String.IsNullOrWhiteSpace analysisResult then
                MessageBox.Show("No results to save. Please analyze some text first.", "Save Error", MessageBoxButtons.OK, MessageBoxIcon.Warning) |> ignore
            else
                
                let saveFileDialog = new SaveFileDialog(Filter = "Text Files (.txt)|.txt")
                if saveFileDialog.ShowDialog() = DialogResult.OK then
                    let filePath = saveFileDialog.FileName
                    File.WriteAllText(filePath, analysisResult)

            MessageBox.Show("Results saved successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information) |> ignore
        else
            MessageBox.Show("Please load and analyze a file first.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning) |> ignore
    )

    showResultsButton.Click.Add(fun _ ->

        let analysisWindowForm = new Form(Text = "Analysis Results", Width = 600, Height = 400)
        

        let resultTextBox = new TextBox(Multiline = true, Dock = DockStyle.Fill, Font = new Font("Arial", 10.0f), ScrollBars = ScrollBars.Vertical)
        analysisWindowForm.Controls.Add(resultTextBox)


        if loadedText <> "" then
            let analysisResult = showAnalysisResults loadedText
            resultTextBox.Text <- analysisResult
        else
            resultTextBox.Text <- "No file loaded yet."


        let closeButton = createStyledButton "Close" 100 Color.LightGray
        closeButton.Click.Add(fun _ -> analysisWindowForm.Close())
        analysisWindowForm.Controls.Add(closeButton)
        analysisWindowForm.AutoSize <- true
        analysisWindowForm.Show()
    )


    showTextButton.Click.Add(fun _ ->

        let textWindowForm = new Form(Text = "Original Text", Width = 600, Height = 400)
        

        let textBox = new TextBox(Multiline = true, Dock = DockStyle.Fill, Font = new Font("Arial", 10.0f), ScrollBars = ScrollBars.Vertical)
        textWindowForm.Controls.Add(textBox)


        if loadedText <> "" then
            textBox.Text <- loadedText
        else
            textBox.Text <- "No file loaded yet."


        let closeButton = createStyledButton "Close" 100 Color.LightGray
        closeButton.Click.Add(fun _ -> textWindowForm.Close())
        textWindowForm.Controls.Add(closeButton)
        textWindowForm.AutoSize <- true
        textWindowForm.Show()
    )

    Application.Run(form)
