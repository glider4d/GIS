<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns="urn:schemas-microsoft-com:office:spreadsheet" xmlns:ss="urn:schemas-microsoft-com:office:spreadsheet" xmlns:x="urn:schemas-microsoft-com:office:excel" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
  <!-- <<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<< Шаблон >>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>> -->
  <xsl:template match="/">
    <xsl:processing-instruction name="mso-application">
      <xsl:text>progid="Excel.Sheet"</xsl:text>
    </xsl:processing-instruction>
    <Workbook>
      <!-- <<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<< Стили шаблона >>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>> -->
      <Styles>
        <Style ss:ID="Bordered1">
          <Borders>
            <Border ss:LineStyle="Continuous" ss:Position="Bottom" ss:Weight="1" />
            <Border ss:LineStyle="Continuous" ss:Position="Left" ss:Weight="1" />
            <Border ss:LineStyle="Continuous" ss:Position="Right" ss:Weight="1" />
            <Border ss:LineStyle="Continuous" ss:Position="Top" ss:Weight="1" />
          </Borders>
        </Style>
        <Style ss:ID="Bordered2">
          <Borders>
            <Border ss:LineStyle="Continuous" ss:Position="Bottom" ss:Weight="2" />
            <Border ss:LineStyle="Continuous" ss:Position="Left" ss:Weight="2" />
            <Border ss:LineStyle="Continuous" ss:Position="Right" ss:Weight="2" />
            <Border ss:LineStyle="Continuous" ss:Position="Top" ss:Weight="2" />
          </Borders>
        </Style>
        <Style ss:ID="Column" ss:Parent="Bordered2">
          <Alignment ss:Horizontal="Center" ss:Vertical="Center" ss:WrapText="1" />
          <Font ss:Color="#000000" ss:FontName="Arial Cyr" x:CharSet="204" />
        </Style>
        <Style ss:ID="Cell1" ss:Parent="Bordered1">
          <Font ss:Bold="1" ss:Color="#000000" ss:FontName="Arial Cyr" x:CharSet="204" />
          <Interior ss:Color="#C0C0C0" ss:Pattern="Solid" />
        </Style>
        <Style ss:ID="Cell2" ss:Parent="Bordered1">
          <Font ss:Bold="1" ss:Color="#003366" ss:FontName="Arial Cyr" x:CharSet="204" />
          <Interior ss:Color="#C0C0C0" ss:Pattern="Solid" />
        </Style>
        <Style ss:ID="Cell3" ss:Parent="Bordered1">
          <Font ss:Bold="1" ss:Color="#333333" ss:FontName="Arial Cyr" x:CharSet="204" />
          <Interior ss:Color="#C0C0C0" ss:Pattern="Solid" />
        </Style>
        <Style ss:ID="Cell4" ss:Parent="Bordered1">
          <Font ss:Color="#000000" ss:FontName="Arial Cyr" x:CharSet="204" />
        </Style>
      </Styles>
      <Worksheet ss:Name="Жилищний фонд">
        <Table>
          <Column ss:Width="100" />
          <Column ss:Width="100" />
          <Column ss:Width="100" />
          <Column ss:Width="150" />
          <Column ss:Width="50" />
          <Column ss:Width="50" />
          <Column ss:Width="50" />
          <Column ss:Width="50" />
          <Column ss:Width="70" />
          <Column ss:Width="70" />
          <Column ss:Width="70" />
          <Column ss:Width="70" />
          <Column ss:Width="70" />
          <Column ss:Width="70" />
          <Column ss:Width="70" />
          <Column ss:Width="70" />
          <Column ss:Width="80" />
          <!-- <<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<< Столбцы таблицы >>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>> -->
          <Row>
            <Cell ss:MergeDown="1" ss:StyleID="Column">
              <Data ss:Type="String">Регион</Data>
            </Cell>
            <Cell ss:MergeDown="1" ss:StyleID="Column">
              <Data ss:Type="String">Нас. пункт</Data>
            </Cell>
            <Cell ss:MergeDown="1" ss:StyleID="Column">
              <Data ss:Type="String">Котельная</Data>
            </Cell>
            <Cell ss:MergeDown="1" ss:StyleID="Column">
              <Data ss:Type="String">Адрес</Data>
            </Cell>
            <Cell ss:MergeDown="1" ss:StyleID="Column">
              <Data ss:Type="String">Кол-во домов</Data>
            </Cell>
            <Cell ss:MergeDown="1" ss:StyleID="Column">
              <Data ss:Type="String">Этаж.</Data>
            </Cell>
            <Cell ss:MergeDown="1" ss:StyleID="Column">
              <Data ss:Type="String">Общая пл.</Data>
            </Cell>
            <Cell ss:MergeDown="1" ss:StyleID="Column">
              <Data ss:Type="String">Кол-во прожив.</Data>
            </Cell>
            <Cell ss:MergeAcross="1" ss:StyleID="Column">
              <Data ss:Type="String">Отопление</Data>
            </Cell>
            <Cell ss:MergeAcross="1" ss:StyleID="Column">
              <Data ss:Type="String">ГВС</Data>
            </Cell>
            <Cell ss:MergeAcross="1" ss:StyleID="Column">
              <Data ss:Type="String">ХВС</Data>
            </Cell>
            <Cell ss:MergeAcross="1" ss:StyleID="Column">
              <Data ss:Type="String">Канализация</Data>
            </Cell>
            <Cell ss:MergeDown="1" ss:StyleID="Column">
              <Data ss:Type="String">Сальдо исход.</Data>
            </Cell>
          </Row>
          <Row>
            <Cell ss:Index="9" ss:StyleID="Column">
              <Data ss:Type="String">Объем</Data>
            </Cell>
            <Cell ss:StyleID="Column">
              <Data ss:Type="String">Сумма</Data>
            </Cell>
            <Cell ss:StyleID="Column">
              <Data ss:Type="String">Объем</Data>
            </Cell>
            <Cell ss:StyleID="Column">
              <Data ss:Type="String">Сумма</Data>
            </Cell>
            <Cell ss:StyleID="Column">
              <Data ss:Type="String">Объем</Data>
            </Cell>
            <Cell ss:StyleID="Column">
              <Data ss:Type="String">Сумма</Data>
            </Cell>
            <Cell ss:StyleID="Column">
              <Data ss:Type="String">Объем</Data>
            </Cell>
            <Cell ss:StyleID="Column">
              <Data ss:Type="String">Сумма</Data>
            </Cell>
          </Row>
          <!-- <<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<< Данные таблицы >>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>> -->
          <xsl:apply-templates select="//Kvp" />
        </Table>
        <WorksheetOptions xmlns="urn:schemas-microsoft-com:office:excel">
          <FreezePanes />
          <FrozenNoSplit />
          <SplitHorizontal>2</SplitHorizontal>
          <TopRowBottomPane>2</TopRowBottomPane>
          <ActivePane>2</ActivePane>
        </WorksheetOptions>
      </Worksheet>
      <Worksheet ss:Name="Договорные подключения">
        <Table>
          <Column ss:Width="100" />
          <Column ss:Width="100" />
          <Column ss:Width="100" />
          <Column ss:Width="150" />
          <Column ss:Width="50" />
          <Column ss:Width="50" />
          <Column ss:Width="50" />
          <Column ss:Width="50" />
          <Column ss:Width="50" />
          <Column ss:Width="50" />
          <Column ss:Width="50" />
          <Column ss:Width="50" />
          <Column ss:Width="50" />
          <Column ss:Width="50" />
          <Column ss:Width="70" />
          <!-- <<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<< Столбцы таблицы >>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>> -->
          <Row>
            <Cell ss:MergeDown="1" ss:StyleID="Column">
              <Data ss:Type="String">Регион</Data>
            </Cell>
            <Cell ss:MergeDown="1" ss:StyleID="Column">
              <Data ss:Type="String">Нас. пункт</Data>
            </Cell>
            <Cell ss:MergeDown="1" ss:StyleID="Column">
              <Data ss:Type="String">Котельная</Data>
            </Cell>
            <Cell ss:MergeDown="1" ss:StyleID="Column">
              <Data ss:Type="String">Наименование</Data>
            </Cell>
            <Cell ss:MergeDown="1" ss:StyleID="Column">
              <Data ss:Type="String">Кол-во объектов</Data>
            </Cell>
            <Cell ss:MergeDown="1" ss:StyleID="Column">
              <Data ss:Type="String">Объем здания</Data>
            </Cell>
            <Cell ss:MergeAcross="1" ss:StyleID="Column">
              <Data ss:Type="String">Отопление</Data>
            </Cell>
            <Cell ss:MergeAcross="1" ss:StyleID="Column">
              <Data ss:Type="String">ГВС</Data>
            </Cell>
            <Cell ss:MergeAcross="1" ss:StyleID="Column">
              <Data ss:Type="String">ХВС</Data>
            </Cell>
            <Cell ss:MergeAcross="1" ss:StyleID="Column">
              <Data ss:Type="String">Канализация</Data>
            </Cell>
            <Cell ss:MergeDown="1" ss:StyleID="Column">
              <Data ss:Type="String">Сумма</Data>
            </Cell>
          </Row>
          <Row>
            <Cell ss:Index="7" ss:StyleID="Column">
              <Data ss:Type="String">Объем</Data>
            </Cell>
            <Cell ss:StyleID="Column">
              <Data ss:Type="String">Сумма</Data>
            </Cell>
            <Cell ss:StyleID="Column">
              <Data ss:Type="String">Объем</Data>
            </Cell>
            <Cell ss:StyleID="Column">
              <Data ss:Type="String">Сумма</Data>
            </Cell>
            <Cell ss:StyleID="Column">
              <Data ss:Type="String">Объем</Data>
            </Cell>
            <Cell ss:StyleID="Column">
              <Data ss:Type="String">Сумма</Data>
            </Cell>
            <Cell ss:StyleID="Column">
              <Data ss:Type="String">Объем</Data>
            </Cell>
            <Cell ss:StyleID="Column">
              <Data ss:Type="String">Сумма</Data>
            </Cell>
          </Row>
          <!-- <<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<< Данные таблицы >>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>> -->
          <xsl:apply-templates select="//Jur" />
        </Table>
      </Worksheet>
    </Workbook>
  </xsl:template>
  <!-- <<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<< Шаблон данных >>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>> -->
  <xsl:template match="Kvp">
    <xsl:choose>
      <xsl:when test="style_id = 1">
        <!-- <<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<< Строка с улусом >>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>> -->
        <Row>
          <Cell ss:MergeAcross="3" ss:StyleID="Cell1">
            <Data ss:Type="String">
              <xsl:value-of select="region" />
            </Data>
          </Cell>
          <Cell ss:StyleID="Cell1">
            <Data ss:Type="Number">
              <xsl:value-of select="house_count" />
            </Data>
          </Cell>
          <Cell ss:StyleID="Cell1">
            <Data ss:Type="String" />
          </Cell>
          <Cell ss:StyleID="Cell1">
            <Data ss:Type="Number">
              <xsl:value-of select="square" />
            </Data>
          </Cell>
          <Cell ss:StyleID="Cell1">
            <Data ss:Type="Number">
              <xsl:value-of select="people_count" />
            </Data>
          </Cell>
          <Cell ss:StyleID="Cell1">
            <Data ss:Type="Number">
              <xsl:value-of select="volume_heat" />
            </Data>
          </Cell>
          <Cell ss:StyleID="Cell1">
            <Data ss:Type="Number">
              <xsl:value-of select="sum_heat" />
            </Data>
          </Cell>
          <Cell ss:StyleID="Cell1">
            <Data ss:Type="Number">
              <xsl:value-of select="volume_hwater" />
            </Data>
          </Cell>
          <Cell ss:StyleID="Cell1">
            <Data ss:Type="Number">
              <xsl:value-of select="sum_hwater" />
            </Data>
          </Cell>
          <Cell ss:StyleID="Cell1">
            <Data ss:Type="Number">
              <xsl:value-of select="volume_cwater" />
            </Data>
          </Cell>
          <Cell ss:StyleID="Cell1">
            <Data ss:Type="Number">
              <xsl:value-of select="sum_cwater" />
            </Data>
          </Cell>
          <Cell ss:StyleID="Cell1">
            <Data ss:Type="Number">
              <xsl:value-of select="volume_sewers" />
            </Data>
          </Cell>
          <Cell ss:StyleID="Cell1">
            <Data ss:Type="Number">
              <xsl:value-of select="sum_sewers" />
            </Data>
          </Cell>
          <Cell ss:StyleID="Cell1">
            <Data ss:Type="Number">
              <xsl:value-of select="saldo_out" />
            </Data>
          </Cell>
        </Row>
      </xsl:when>
      <xsl:when test="style_id = 2">
        <!-- <<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<< Строка с населенным пунктом >>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>> -->
        <Row>
          <Cell ss:Index="2" ss:MergeAcross="2" ss:StyleID="Cell2">
            <Data ss:Type="String">
              <xsl:value-of select="city" />
            </Data>
          </Cell>
          <Cell ss:StyleID="Cell2">
            <Data ss:Type="Number">
              <xsl:value-of select="house_count" />
            </Data>
          </Cell>
          <Cell ss:StyleID="Cell2">
            <Data ss:Type="String" />
          </Cell>
          <Cell ss:StyleID="Cell2">
            <Data ss:Type="Number">
              <xsl:value-of select="square" />
            </Data>
          </Cell>
          <Cell ss:StyleID="Cell2">
            <Data ss:Type="Number">
              <xsl:value-of select="people_count" />
            </Data>
          </Cell>
          <Cell ss:StyleID="Cell2">
            <Data ss:Type="Number">
              <xsl:value-of select="volume_heat" />
            </Data>
          </Cell>
          <Cell ss:StyleID="Cell2">
            <Data ss:Type="Number">
              <xsl:value-of select="sum_heat" />
            </Data>
          </Cell>
          <Cell ss:StyleID="Cell2">
            <Data ss:Type="Number">
              <xsl:value-of select="volume_hwater" />
            </Data>
          </Cell>
          <Cell ss:StyleID="Cell2">
            <Data ss:Type="Number">
              <xsl:value-of select="sum_hwater" />
            </Data>
          </Cell>
          <Cell ss:StyleID="Cell2">
            <Data ss:Type="Number">
              <xsl:value-of select="volume_cwater" />
            </Data>
          </Cell>
          <Cell ss:StyleID="Cell2">
            <Data ss:Type="Number">
              <xsl:value-of select="sum_cwater" />
            </Data>
          </Cell>
          <Cell ss:StyleID="Cell2">
            <Data ss:Type="Number">
              <xsl:value-of select="volume_sewers" />
            </Data>
          </Cell>
          <Cell ss:StyleID="Cell2">
            <Data ss:Type="Number">
              <xsl:value-of select="sum_sewers" />
            </Data>
          </Cell>
          <Cell ss:StyleID="Cell2">
            <Data ss:Type="Number">
              <xsl:value-of select="saldo_out" />
            </Data>
          </Cell>
        </Row>
      </xsl:when>
      <xsl:when test="style_id = 3">
        <!-- <<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<< Строка с котельной >>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>> -->
        <Row>
          <Cell ss:Index="3" ss:MergeAcross="1" ss:StyleID="Cell3">
            <Data ss:Type="String">
              <xsl:value-of select="boiler" />
            </Data>
          </Cell>
          <Cell ss:StyleID="Cell3">
            <Data ss:Type="Number">
              <xsl:value-of select="house_count" />
            </Data>
          </Cell>
          <Cell ss:StyleID="Cell3">
            <Data ss:Type="String" />
          </Cell>
          <Cell ss:StyleID="Cell3">
            <Data ss:Type="Number">
              <xsl:value-of select="square" />
            </Data>
          </Cell>
          <Cell ss:StyleID="Cell3">
            <Data ss:Type="Number">
              <xsl:value-of select="people_count" />
            </Data>
          </Cell>
          <Cell ss:StyleID="Cell3">
            <Data ss:Type="Number">
              <xsl:value-of select="volume_heat" />
            </Data>
          </Cell>
          <Cell ss:StyleID="Cell3">
            <Data ss:Type="Number">
              <xsl:value-of select="sum_heat" />
            </Data>
          </Cell>
          <Cell ss:StyleID="Cell3">
            <Data ss:Type="Number">
              <xsl:value-of select="volume_hwater" />
            </Data>
          </Cell>
          <Cell ss:StyleID="Cell3">
            <Data ss:Type="Number">
              <xsl:value-of select="sum_hwater" />
            </Data>
          </Cell>
          <Cell ss:StyleID="Cell3">
            <Data ss:Type="Number">
              <xsl:value-of select="volume_cwater" />
            </Data>
          </Cell>
          <Cell ss:StyleID="Cell3">
            <Data ss:Type="Number">
              <xsl:value-of select="sum_cwater" />
            </Data>
          </Cell>
          <Cell ss:StyleID="Cell3">
            <Data ss:Type="Number">
              <xsl:value-of select="volume_sewers" />
            </Data>
          </Cell>
          <Cell ss:StyleID="Cell3">
            <Data ss:Type="Number">
              <xsl:value-of select="sum_sewers" />
            </Data>
          </Cell>
          <Cell ss:StyleID="Cell3">
            <Data ss:Type="Number">
              <xsl:value-of select="saldo_out" />
            </Data>
          </Cell>
        </Row>
      </xsl:when>
      <xsl:when test="style_id = 4">
        <!-- <<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<< Строка с объектами >>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>> -->
        <Row>
          <Cell ss:MergeAcross="1" ss:Index="4" ss:StyleID="Cell4">
            <Data ss:Type="String">
              <xsl:value-of select="address" />
            </Data>
          </Cell>
          <Cell ss:StyleID="Cell4">
            <Data ss:Type="Number">
              <xsl:value-of select="floors" />
            </Data>
          </Cell>
          <Cell ss:StyleID="Cell4">
            <Data ss:Type="Number">
              <xsl:value-of select="square" />
            </Data>
          </Cell>
          <Cell ss:StyleID="Cell4">
            <Data ss:Type="Number">
              <xsl:value-of select="people_count" />
            </Data>
          </Cell>
          <Cell ss:StyleID="Cell4">
            <Data ss:Type="Number">
              <xsl:value-of select="volume_heat" />
            </Data>
          </Cell>
          <Cell ss:StyleID="Cell4">
            <Data ss:Type="Number">
              <xsl:value-of select="sum_heat" />
            </Data>
          </Cell>
          <Cell ss:StyleID="Cell4">
            <Data ss:Type="Number">
              <xsl:value-of select="volume_hwater" />
            </Data>
          </Cell>
          <Cell ss:StyleID="Cell4">
            <Data ss:Type="Number">
              <xsl:value-of select="sum_hwater" />
            </Data>
          </Cell>
          <Cell ss:StyleID="Cell4">
            <Data ss:Type="Number">
              <xsl:value-of select="volume_cwater" />
            </Data>
          </Cell>
          <Cell ss:StyleID="Cell4">
            <Data ss:Type="Number">
              <xsl:value-of select="sum_cwater" />
            </Data>
          </Cell>
          <Cell ss:StyleID="Cell4">
            <Data ss:Type="Number">
              <xsl:value-of select="volume_sewers" />
            </Data>
          </Cell>
          <Cell ss:StyleID="Cell4">
            <Data ss:Type="Number">
              <xsl:value-of select="sum_sewers" />
            </Data>
          </Cell>
          <Cell ss:StyleID="Cell4">
            <Data ss:Type="Number">
              <xsl:value-of select="saldo_out" />
            </Data>
          </Cell>
        </Row>
      </xsl:when>
    </xsl:choose>
  </xsl:template>
  <xsl:template match="Jur">
    <xsl:choose>
      <xsl:when test="style_id = 1">
        <!-- <<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<< Строка с улусом >>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>> -->
        <Row>
          <Cell ss:MergeAcross="3" ss:StyleID="Cell1">
            <Data ss:Type="String">
              <xsl:value-of select="reg" />
            </Data>
          </Cell>
          <Cell ss:StyleID="Cell1">
            <Data ss:Type="Number">
              <xsl:value-of select="count" />
            </Data>
          </Cell>
          <Cell ss:StyleID="Cell1">
            <Data ss:Type="Number">
              <xsl:value-of select="building_volume" />
            </Data>
          </Cell>
          <Cell ss:StyleID="Cell1">
            <Data ss:Type="Number">
              <xsl:value-of select="heat_volume" />
            </Data>
          </Cell>
          <Cell ss:StyleID="Cell1">
            <Data ss:Type="Number">
              <xsl:value-of select="heat_summa" />
            </Data>
          </Cell>
          <Cell ss:StyleID="Cell1">
            <Data ss:Type="Number">
              <xsl:value-of select="hw_vol" />
            </Data>
          </Cell>
          <Cell ss:StyleID="Cell1">
            <Data ss:Type="Number">
              <xsl:value-of select="hw_summa" />
            </Data>
          </Cell>
          <Cell ss:StyleID="Cell1">
            <Data ss:Type="Number">
              <xsl:value-of select="vol_xvs" />
            </Data>
          </Cell>
          <Cell ss:StyleID="Cell1">
            <Data ss:Type="Number">
              <xsl:value-of select="sum_xvs" />
            </Data>
          </Cell>
          <Cell ss:StyleID="Cell1">
            <Data ss:Type="Number">
              <xsl:value-of select="vol_kan" />
            </Data>
          </Cell>
          <Cell ss:StyleID="Cell1">
            <Data ss:Type="Number">
              <xsl:value-of select="sum_kan" />
            </Data>
          </Cell>
          <Cell ss:StyleID="Cell1">
            <Data ss:Type="Number">
              <xsl:value-of select="all_summa" />
            </Data>
          </Cell>
        </Row>
      </xsl:when>
      <xsl:when test="style_id = 2">
        <!-- <<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<< Строка с населенным пунктом >>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>> -->
        <Row>
          <Cell ss:Index="2" ss:MergeAcross="2" ss:StyleID="Cell2">
            <Data ss:Type="String">
              <xsl:value-of select="city" />
            </Data>
          </Cell>
          <Cell ss:StyleID="Cell2">
            <Data ss:Type="Number">
              <xsl:value-of select="count" />
            </Data>
          </Cell>
          <Cell ss:StyleID="Cell2">
            <Data ss:Type="Number">
              <xsl:value-of select="building_volume" />
            </Data>
          </Cell>
          <Cell ss:StyleID="Cell2">
            <Data ss:Type="Number">
              <xsl:value-of select="heat_volume" />
            </Data>
          </Cell>
          <Cell ss:StyleID="Cell2">
            <Data ss:Type="Number">
              <xsl:value-of select="heat_summa" />
            </Data>
          </Cell>
          <Cell ss:StyleID="Cell2">
            <Data ss:Type="Number">
              <xsl:value-of select="hw_vol" />
            </Data>
          </Cell>
          <Cell ss:StyleID="Cell2">
            <Data ss:Type="Number">
              <xsl:value-of select="hw_summa" />
            </Data>
          </Cell>
          <Cell ss:StyleID="Cell2">
            <Data ss:Type="Number">
              <xsl:value-of select="vol_xvs" />
            </Data>
          </Cell>
          <Cell ss:StyleID="Cell2">
            <Data ss:Type="Number">
              <xsl:value-of select="sum_xvs" />
            </Data>
          </Cell>
          <Cell ss:StyleID="Cell2">
            <Data ss:Type="Number">
              <xsl:value-of select="vol_kan" />
            </Data>
          </Cell>
          <Cell ss:StyleID="Cell2">
            <Data ss:Type="Number">
              <xsl:value-of select="sum_kan" />
            </Data>
          </Cell>
          <Cell ss:StyleID="Cell1">
            <Data ss:Type="Number">
              <xsl:value-of select="all_summa" />
            </Data>
          </Cell>
        </Row>
      </xsl:when>
      <xsl:when test="style_id = 3">
        <!-- <<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<< Строка с котельной >>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>> -->
        <Row>
          <Cell ss:Index="3" ss:MergeAcross="1" ss:StyleID="Cell3">
            <Data ss:Type="String">
              <xsl:value-of select="boiler" />
            </Data>
          </Cell>
          <Cell ss:StyleID="Cell3">
            <Data ss:Type="Number">
              <xsl:value-of select="count" />
            </Data>
          </Cell>
          <Cell ss:StyleID="Cell3">
            <Data ss:Type="Number">
              <xsl:value-of select="building_volume" />
            </Data>
          </Cell>
          <Cell ss:StyleID="Cell3">
            <Data ss:Type="Number">
              <xsl:value-of select="heat_volume" />
            </Data>
          </Cell>
          <Cell ss:StyleID="Cell3">
            <Data ss:Type="Number">
              <xsl:value-of select="heat_summa" />
            </Data>
          </Cell>
          <Cell ss:StyleID="Cell3">
            <Data ss:Type="Number">
              <xsl:value-of select="hw_vol" />
            </Data>
          </Cell>
          <Cell ss:StyleID="Cell3">
            <Data ss:Type="Number">
              <xsl:value-of select="hw_summa" />
            </Data>
          </Cell>
          <Cell ss:StyleID="Cell3">
            <Data ss:Type="Number">
              <xsl:value-of select="vol_xvs" />
            </Data>
          </Cell>
          <Cell ss:StyleID="Cell3">
            <Data ss:Type="Number">
              <xsl:value-of select="sum_xvs" />
            </Data>
          </Cell>
          <Cell ss:StyleID="Cell3">
            <Data ss:Type="Number">
              <xsl:value-of select="vol_kan" />
            </Data>
          </Cell>
          <Cell ss:StyleID="Cell3">
            <Data ss:Type="Number">
              <xsl:value-of select="sum_kan" />
            </Data>
          </Cell>
          <Cell ss:StyleID="Cell1">
            <Data ss:Type="Number">
              <xsl:value-of select="all_summa" />
            </Data>
          </Cell>
        </Row>
      </xsl:when>
      <xsl:when test="style_id = 4">
        <!-- <<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<< Строка с объектами >>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>> -->
        <Row>
          <Cell ss:MergeAcross="1" ss:Index="4" ss:StyleID="Cell4">
            <Data ss:Type="String">
              <xsl:value-of select="name" />
            </Data>
          </Cell>
          <Cell ss:StyleID="Cell4">
            <Data ss:Type="Number">
              <xsl:value-of select="building_volume" />
            </Data>
          </Cell>
          <Cell ss:StyleID="Cell4">
            <Data ss:Type="Number">
              <xsl:value-of select="heat_volume" />
            </Data>
          </Cell>
          <Cell ss:StyleID="Cell4">
            <Data ss:Type="Number">
              <xsl:value-of select="heat_summa" />
            </Data>
          </Cell>
          <Cell ss:StyleID="Cell4">
            <Data ss:Type="Number">
              <xsl:value-of select="hw_vol" />
            </Data>
          </Cell>
          <Cell ss:StyleID="Cell4">
            <Data ss:Type="Number">
              <xsl:value-of select="hw_summa" />
            </Data>
          </Cell>
          <Cell ss:StyleID="Cell4">
            <Data ss:Type="Number">
              <xsl:value-of select="vol_xvs" />
            </Data>
          </Cell>
          <Cell ss:StyleID="Cell4">
            <Data ss:Type="Number">
              <xsl:value-of select="sum_xvs" />
            </Data>
          </Cell>
          <Cell ss:StyleID="Cell4">
            <Data ss:Type="Number">
              <xsl:value-of select="vol_kan" />
            </Data>
          </Cell>
          <Cell ss:StyleID="Cell4">
            <Data ss:Type="Number">
              <xsl:value-of select="sum_kan" />
            </Data>
          </Cell>
          <Cell ss:StyleID="Cell1">
            <Data ss:Type="Number">
              <xsl:value-of select="all_summa" />
            </Data>
          </Cell>
        </Row>
      </xsl:when>
    </xsl:choose>
  </xsl:template>
</xsl:stylesheet>