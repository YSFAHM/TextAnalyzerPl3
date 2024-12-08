module Gui

open System
open System.IO
open System.Windows.Forms
open System.Drawing

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