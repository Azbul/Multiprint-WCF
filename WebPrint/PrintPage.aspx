<%@ Page Language="C#" CodeBehind="~/PrintPage.cs" Inherits="WebPrint.PrintPage"%>

<!DOCTYPE html>

<html>
<head runat="server">
    <title>Print Service</title>
    <link href="/resources/css/examples.css" rel="stylesheet" />
    <style>
        .list-item {
            font:normal 11px tahoma, arial, helvetica, sans-serif;
            padding:3px 10px 3px 10px;
            border:1px solid #fff;
            border-bottom:1px solid #eeeeee;
            white-space:normal;
            color:#555;
        }

        .list-item h3 {
            display:block;
            font:inherit;
            font-weight:bold;
            margin:0px;
            color:#222;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server"> 
    <ext:ResourceManager runat="server" Theme="Triton"/>
    
            
    <ext:Store ID="PrinterStore" runat="server">
        <Model>
            <ext:Model ID="model1" runat="server" IDProperty="pid">
                <Fields>
                    <ext:ModelField Name="pid" Type="Int"/>
                    <ext:ModelField Name="prname" Type="String"/>
                    <ext:ModelField Name="pcname" Type="String"/>
                    <ext:ModelField Name="status" Type="String"/>
                </Fields>
            </ext:Model>
        </Model>
    </ext:Store>
             
    <ext:Viewport runat="server">
        <Items>
           <ext:Panel 
            ID="Window1"
            runat="server" 
            Title="Печать"
            Height="400"
            Width="600"
            Frame="true"
            Align="Start"
            Draggable="true"
            Cls="box"
            BodyPadding="5"
            DefaultButton="0"
            Layout="AnchorLayout"
            Icon="Printer"
            DefaultAnchor="100%">
            <Items>
                 <ext:Container runat="server" Layout="HBoxLayout" MarginSpec="0 0 10">
                    <Items>
                        <ext:FieldSet
                            runat="server"
                            Flex="1"
                            Title="Выбрать принтер"
                            Layout="AnchorLayout"
                            Height="200"
                            DefaultAnchor="100%">

                            <Items>
                                <ext:ComboBox
                                ID="SelectPrinterComboBox"
                                runat="server"
                                Width="500"
                                Editable="false"
                                DisplayField="prname"
                                ValueField="pid"
                                QueryMode="Local"
                                ForceSelection="true"
                                TriggerAction="All"
                                Icon="PrinterEmpty"
                                StoreID="PrinterStore"
                                EmptyText="Выберите принтер...">

                                <ListConfig>
                                    <ItemTpl runat="server">
                                        <Html>
                                            <div class="list-item">
                                                <h3>{prname}</h3>
                                                Состояние: {status:ellipsis(50)}
                                            </div>
                                        </Html>
                                    </ItemTpl>
                                </ListConfig>
                                <DirectEvents>
                                    <Select OnEvent="OnComboBoxSelected" />
                                </DirectEvents>
                             </ext:ComboBox>

                            <ext:TextField
                            ID="StatusField"
                            runat="server"
                            Name="st"
                            ReadOnly="true"
                            FieldLabel="Состояние"
                            Width="260"
                            EmptyText="Неизвестно"
                            />

                            <ext:TextField
                            ID="PcNameField"
                            runat="server"
                            Name="pcname"
                            ReadOnly="true"
                            FieldLabel="Компьютер"
                            Width="260"
                            EmptyText="Неизвестно"
                            />
                            </Items>
                        </ext:FieldSet>

                        <ext:Component runat="server" Width="10" />

                        <ext:FieldSet
                            runat="server"
                            Flex="1"
                            Title="Диапазон страниц"
                            Height="200"
                            Layout="AnchorLayout"
                            DefaultAnchor="100%">

                            <Items>
                                <ext:Radio runat="server" ID="AllPages" BoxLabel="Все" Name="pages" Checked="true">
                                <Listeners>
                                     <Change Handler="#{pagesField}.enable();" />
                                    </Listeners>
                                </ext:Radio>
                                <ext:Radio runat="server" ID="Any" BoxLabel="Страницы:" Name="pages">
                                    <Listeners>
                                     <Change Handler="#{pagesField}.disable();" />
                                    </Listeners>
                                </ext:Radio>
                                <ext:TextField 
                                    ID="PagesField"
                                    runat="server" 
                                    Disabled="true" 
                                    MaskRe="/[0-9,-]/"
                                    EmptyText="1-5"/>
                            </Items>
                        </ext:FieldSet>
                    </Items>
                </ext:Container>
                <ext:TextField
                            ID="LogField"
                            runat="server"
                            Name="log"
                            ReadOnly="true"
                            FieldLabel="Log"
                            Width="260"
                            EmptyText="log field" />  
                <ext:FileUploadField ID="UploadField" runat="server" FieldLabel="Выбрать файл" EmptyText="Файл не выбран" Accept="application/pdf" Icon="Attach"  />
            </Items>
            <Buttons>
                <ext:Button 
                    ID="Button2"
                    runat="server" 
                    Text="Обновить"
                    Icon="ArrowRefresh" 
                    OnDirectClick="Refresh_Click"/>
                <ext:Button 
                    ID="Button1"
                    runat="server" 
                    Text="Печать"
                    Icon="Printer" 
                    OnDirectClick="Print_Click"/>
            </Buttons>
                            
        </ext:Panel>
        </Items>
    </ext:Viewport>
      
    <ext:Window
    runat="server"
    Title="Мои документы"
    Width="800"
    Height="400"
    MinWidth="300"
    MinHeight="200"
    X="610"
    Y="0"
    Closable="false"
    Layout="FitLayout"
    >
        <Items>
        <ext:GridPanel
        ID="GridPanel1"
        runat="server"
        ForceFit="true"
        Width="800" 
        Height="400">
        <Store>
            <ext:Store ID="MyFilesStore" runat="server">
                <Model>
                    <ext:Model runat="server">
                        <Fields>
                            <ext:ModelField Name="docname" Type="Auto"/>
                            <ext:ModelField Name="filestatus" Type="String" />
                            <ext:ModelField Name="prname" Type="String" />
                            <ext:ModelField Name="pagetoprint" Type="String" />
                            <ext:ModelField Name="pcname" Type="Auto" />
                            <ext:ModelField Name="datetime" Type="String"/>
                        </Fields>
                    </ext:Model>
                </Model>
            </ext:Store>
        </Store>
        <ColumnModel runat="server">
            <Columns>
                <ext:Column
                    ID="DocColumn"
                    runat="server"
                    Text="Документ"
                    Width="110"
                    DataIndex="docname"
                    />

                <ext:Column
                    ID="StatusColumn"
                    runat="server"
                    Text="Статус файла"
                    Width="110"
                    DataIndex="filestatus"
                     />

                <ext:Column                                                          
                    ID="PrnameColumn"                                                
                    runat="server"                                                   
                    Text="Принтер"                                                   
                    Width="75"                                                       
                    DataIndex="prname"                                               
                    />

                <ext:Column
                    ID="PageToPrintColumn"
                    runat="server"
                    Text="Страницы на печать"
                    Width="70"
                    DataIndex="pagetoprint"
                    
                    />

                <ext:Column
                    ID="PcnameColumn"
                    runat="server"
                    Text="Компьютер"
                    Width="80"
                    DataIndex="pcname"
                    />

                <ext:Column
                    ID="DateTimeColumn"
                    runat="server"
                    Text="Поставлено в очередь"
                    Width="130"
                    DataIndex="datetime"
                    />
            </Columns>

        </ColumnModel>
        <View>
            <ext:GridView runat="server" StripeRows="true" TrackOver="true" />
        </View>
        </ext:GridPanel>
    </Items>
        <Buttons>
                <ext:Button 
                    ID="Button3"
                    runat="server" 
                    Text="Обновить все таблицы"
                    Icon="ArrowRefresh" 
                    OnDirectClick="RefreshAllQueueTable_Click"/>
                
            </Buttons>
 </ext:Window>

         <ext:Window
    runat="server"
    Title="Таблица принтеров"
    Width="600"
    Height="400"
    MinWidth="300"
    MinHeight="200"
    X="0"
    Y="405"
    Closable="false"
    Layout="FitLayout"
    >
        <Items>
        <ext:GridPanel
        ID="GridPanel2"
        runat="server"
        ForceFit="true"
        StoreID="PrinterStore"
        Width="600" 
        Height="400">
        
        <ColumnModel runat="server">
            <Columns>
                <ext:Column
                    ID="Column1"
                    runat="server"
                    Text="ID"
                    Width="50"
                    DataIndex="pid"
                    />

                <ext:Column
                    ID="Column2"
                    runat="server"
                    Text="Принтер"
                    Width="150"
                    DataIndex="prname"
                     />

                <ext:Column                                                          
                    ID="Column3"                                                
                    runat="server"                                                   
                    Text="Компьютер"                                                   
                    Width="150"                                                       
                    DataIndex="pcname"                                               
                    />

                <ext:Column
                    ID="Column4"
                    runat="server"
                    Text="Статус"
                    Width="50"
                    DataIndex="status"
                    />

            </Columns>

        </ColumnModel>
        <View>
            <ext:GridView runat="server" StripeRows="true" TrackOver="true" />
        </View>
        </ext:GridPanel>
    </Items>
 </ext:Window>

        <ext:Window
    runat="server"
    Title="Таблица всех очередей"
    Width="800"
    Height="400"
    MinWidth="300"
    MinHeight="200"
    X="610"
    Y="405"
    Closable="false"
    Layout="FitLayout"
    >
        <Items>
        <ext:GridPanel
        ID="GridPanel3"
        runat="server"
        ForceFit="true"
        Width="800" 
        Height="400">
        <Store>
            <ext:Store ID="QueueStore" runat="server">
                <Model>
                    <ext:Model runat="server" IDProperty="qid">
                        <Fields>
                            <ext:ModelField Name="qid" Type="Int"/>
                            <ext:ModelField Name="pagefrom" Type="Int"/>
                            <ext:ModelField Name="pageto" Type="Int"/>
                            <ext:ModelField Name="printpages" Type="String"/>
                            <ext:ModelField Name="printerid" Type="Int"/>
                            <ext:ModelField Name="docname" Type="String"/>
                            <ext:ModelField Name="filestatus" Type="String" />
                            <ext:ModelField Name="printedconfim" Type="Int" />
                            <ext:ModelField Name="pcname" Type="String" />
                            <ext:ModelField Name="datetime" Type="String"/>
                        </Fields>
                    </ext:Model>
                </Model>
            </ext:Store>
        </Store>
        <ColumnModel runat="server">
            <Columns>
                <ext:Column
                    ID="Column5"
                    runat="server"
                    Text="ID"
                    Width="50"
                    DataIndex="qid"
                    />

                <ext:Column
                    ID="Column6"
                    runat="server"
                    Text="Первая страница"
                    Width="110"
                    DataIndex="pagefrom"
                     />

                <ext:Column                                                          
                    ID="Column7"                                                
                    runat="server"                                                   
                    Text="Конечная страница"                                                   
                    Width="75"                                                       
                    DataIndex="pageto"                                               
                    />

                <ext:Column
                    ID="Column8"
                    runat="server"
                    Text="Пользовательские границы"
                    Width="70"
                    DataIndex="printpages"
                    
                    />

                <ext:Column
                    ID="Column9"
                    runat="server"
                    Text="ID принтера"
                    Width="80"
                    DataIndex="printerid"
                    />

                <ext:Column
                    ID="Column11"
                    runat="server"
                    Text="Документ"
                    Width="80"
                    DataIndex="docname"
                    />

                <ext:Column
                    ID="Column12"
                    runat="server"
                    Text="Статус файла"
                    Width="80"
                    DataIndex="filestatus"
                    />

                <ext:Column
                    ID="Column14"
                    runat="server"
                    Text="Подверждение печати"
                    Width="80"
                    DataIndex="printedconfim"
                    />

                <ext:Column
                    ID="Column15"
                    runat="server"
                    Text="ПК клиента"
                    Width="80"
                    DataIndex="pcname"
                    />

                <ext:Column
                    ID="Column10"
                    runat="server"
                    Text="Поставлено в очередь"
                    Width="130"
                    DataIndex="datetime"
                    />
            </Columns>

        </ColumnModel>
        <View>
            <ext:GridView runat="server" StripeRows="true" TrackOver="true" />
        </View>
        </ext:GridPanel>
    </Items>
 </ext:Window>

 </form>
</body>
</html>