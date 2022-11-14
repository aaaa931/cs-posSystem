# cs-posSystem

> 本專案為「C#, .NET 程式開發與應用」課程的作品，主要功能為倉儲系統，紀錄各產品進貨、出貨狀況，目前有多少庫存，支援匯出 word、pdf、excel 檔案，繪製統計圖表等主要功能，且包含安裝與解除安裝等其他額外功能

## 功能列表

* SQL 資料庫
* 匯出 wrod、pdf、excel 檔案
* 產出統計圖表
* 登入功能
* 安裝、解除安裝

## 檔案說明

1. cs-posSystem 專案

* Form1.cs

主畫面，所有介面基本上都在此，包含各式 Button、menuStrip click 事件，各 tab 呈現畫面等

* Form2.cs

登入畫面，驗證密碼是否正確

* form_word.cs

[測試檔] 使用 richTextBox 開啟 word 檔案

# form_pdf.cs

[測試檔] 使用 Adobe PDF Reader 開啟 pdf 檔案

# form_excel.cs

[測試檔] 使用 dataGridView 開啟 excel 檔案，此方法不支援設計格式，需修改

2. Setup 專案

專案安裝所需 dll 檔案、icon 等
