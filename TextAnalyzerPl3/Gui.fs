﻿module Gui

open System
open System.IO
open System.Windows.Forms
open System.Drawing
open Analysis


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

    
    let analyzeButton = createStyledButton "Analyze" 150 Color.LightGreen
    let loadButton = createStyledButton "Load File" 150 Color.LightBlue
    let clearButton = createStyledButton "Clear Text" 150 Color.Orange
    let saveButton = createStyledButton "Save Results" 150 Color.LightSalmon
    let showResultsButton = createStyledButton "Show Results" 150 Color.LightCoral
    let showTextButton = createStyledButton "Show Text" 150 Color.LightYellow

    
    flowLayout.Controls.Add(analyzeButton)
    flowLayout.Controls.Add(loadButton)
    flowLayout.Controls.Add(clearButton)
    flowLayout.Controls.Add(saveButton)
    flowLayout.Controls.Add(showResultsButton)
    flowLayout.Controls.Add(showTextButton)


    Application.Run(form)
