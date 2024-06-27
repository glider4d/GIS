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
        <Style ss:ID="Title">
          <Alignment ss:Horizontal="Center" />
          <Font ss:Color="#000000" ss:FontName="Arial Cyr" x:CharSet="204" />
        </Style>
        <Style ss:ID="Column" ss:Parent="Bordered2">
          <Alignment ss:Horizontal="Center" ss:Vertical="Center" ss:WrapText="1" />
          <Font ss:Color="#000000" ss:FontName="Arial Cyr" x:CharSet="204" />
        </Style>
        <Style ss:ID="Cell" ss:Parent="Bordered1">
          <Font ss:Color="#000000" ss:FontName="Arial Cyr" x:CharSet="204" />
        </Style>
      </Styles>
      <Worksheet ss:Name="Отчет">
        <Table>
          <Column ss:Width="96" />
          <Column ss:Width="58.5" />
          <Column ss:Width="78.75" />
          <Column ss:Width="44.25" />
          <Column ss:Width="78.75" />
          <Column ss:Width="97.5" />
          <Column ss:Width="88.5" />
          <Column ss:Width="81.75" />
          <Column ss:Width="84.75" />
          <Column ss:Width="81.75" />
          <!-- <<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<< Заголовок >>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>> -->
          <Row>
            <Cell ss:Index="6" ss:StyleID="Title">
              <Data ss:Type="String">Информация о количестве введенных объектов в "Инженерно-картографическую систему"</Data>
            </Cell>
          </Row>
          <Row>
            <Cell ss:Index="6" ss:StyleID="Title">
              <Data ss:Type="String">по состоянию на</Data>
            </Cell>
          </Row>
          <!-- <<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<< Столбцы таблицы >>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>> -->
          <Row ss:Index="4">
            <Cell ss:MergeDown="1" ss:StyleID="Column">
              <Data ss:Type="String">Населенный пункт</Data>
            </Cell>
            <Cell ss:MergeDown="1" ss:StyleID="Column">
              <Data ss:Type="String">Котельные</Data>
            </Cell>
            <Cell ss:MergeDown="1" ss:StyleID="Column">
              <Data ss:Type="String">Общественные здания</Data>
            </Cell>
            <Cell ss:MergeDown="1" ss:StyleID="Column">
              <Data ss:Type="String">Жилой фонд</Data>
            </Cell>
            <Cell ss:MergeDown="1" ss:StyleID="Column">
              <Data ss:Type="String">Хозяйственные нужды</Data>
            </Cell>
            <Cell ss:MergeDown="1" ss:StyleID="Column">
              <Data ss:Type="String">Производственные нужды</Data>
            </Cell>
            <Cell ss:MergeAcross="3" ss:StyleID="Column">
              <Data ss:Type="String">Сети</Data>
            </Cell>
          </Row>
          <Row ss:Height="39.75">
            <Cell ss:Index="7" ss:StyleID="Column">
              <Data ss:Type="String">Теплоснабжение</Data>
            </Cell>
            <Cell ss:StyleID="Column">
              <Data ss:Type="String">Горячее водоснабжение</Data>
            </Cell>
            <Cell ss:StyleID="Column">
              <Data ss:Type="String">Холодное водоснабжение</Data>
            </Cell>
            <Cell ss:StyleID="Column">
              <Data ss:Type="String">Водоотведение</Data>
            </Cell>
          </Row>
          <!-- <<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<< Данные таблицы >>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>> -->
          <xsl:apply-templates select="//Data" />
        </Table>
      </Worksheet>
    </Workbook>
  </xsl:template>
  <!-- <<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<< Шаблон данных >>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>> -->
  <xsl:template match="Data">
    <Row>
      <Cell ss:StyleID="Cell">
        <Data ss:Type="String">
          <xsl:value-of select="city" />
        </Data>
      </Cell>
      <Cell ss:StyleID="Cell">
        <Data ss:Type="Number">
          <xsl:value-of select="a" />
        </Data>
      </Cell>
      <Cell ss:StyleID="Cell">
        <Data ss:Type="Number">
          <xsl:value-of select="b" />
        </Data>
      </Cell>
      <Cell ss:StyleID="Cell">
        <Data ss:Type="Number">
          <xsl:value-of select="c" />
        </Data>
      </Cell>
      <Cell ss:StyleID="Cell">
        <Data ss:Type="Number">
          <xsl:value-of select="d" />
        </Data>
      </Cell>
      <Cell ss:StyleID="Cell">
        <Data ss:Type="Number">
          <xsl:value-of select="e" />
        </Data>
      </Cell>
      <Cell ss:StyleID="Cell">
        <Data ss:Type="Number">
          <xsl:value-of select="f" />
        </Data>
      </Cell>
      <Cell ss:StyleID="Cell">
        <Data ss:Type="Number">
          <xsl:value-of select="g" />
        </Data>
      </Cell>
      <Cell ss:StyleID="Cell">
        <Data ss:Type="Number">
          <xsl:value-of select="h" />
        </Data>
      </Cell>
      <Cell ss:StyleID="Cell">
        <Data ss:Type="Number">
          <xsl:value-of select="i" />
        </Data>
      </Cell>
    </Row>
  </xsl:template>
</xsl:stylesheet>