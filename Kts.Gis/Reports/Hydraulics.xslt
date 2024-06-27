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
        <Style ss:ID="Cell" ss:Parent="Bordered1">
          <Font ss:Color="#000000" ss:FontName="Arial Cyr" x:CharSet="204" />
        </Style>
      </Styles>
      <Worksheet ss:Name="Отчет">
        <Table>
          <Column ss:Width="100" />
          <Column ss:Width="100" />
          <Column ss:Width="100" />
          <Column ss:Width="100" />
          <Column ss:Width="100" />
          <Column ss:Width="100" />
          <Column ss:Width="100" />
          <Column ss:Width="100" />
          <Column ss:Width="100" />
          <Column ss:Width="100" />
          <!-- <<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<< Столбцы таблицы >>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>> -->
          <Row>
            <Cell ss:StyleID="Column">
              <Data ss:Type="String">road</Data>
            </Cell>
            <Cell ss:StyleID="Column">
              <Data ss:Type="String">Q</Data>
            </Cell>
            <Cell ss:StyleID="Column">
              <Data ss:Type="String">G</Data>
            </Cell>
            <Cell ss:StyleID="Column">
              <Data ss:Type="String">L</Data>
            </Cell>
            <Cell ss:StyleID="Column">
              <Data ss:Type="String">Le</Data>
            </Cell>
            <Cell ss:StyleID="Column">
              <Data ss:Type="String">L+Le</Data>
            </Cell>
            <Cell ss:StyleID="Column">
              <Data ss:Type="String">D</Data>
            </Cell>
            <Cell ss:StyleID="Column">
              <Data ss:Type="String">v</Data>
            </Cell>
            <Cell ss:StyleID="Column">
              <Data ss:Type="String">R</Data>
            </Cell>
            <Cell ss:StyleID="Column">
              <Data ss:Type="String">H</Data>
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
          <xsl:value-of select="road" />
        </Data>
      </Cell>
      <Cell ss:StyleID="Cell">
        <Data ss:Type="Number">
          <xsl:value-of select="Q" />
        </Data>
      </Cell>
      <Cell ss:StyleID="Cell">
        <Data ss:Type="Number">
          <xsl:value-of select="G" />
        </Data>
      </Cell>
      <Cell ss:StyleID="Cell">
        <Data ss:Type="Number">
          <xsl:value-of select="L" />
        </Data>
      </Cell>
      <Cell ss:StyleID="Cell">
        <Data ss:Type="Number">
          <xsl:value-of select="Le" />
        </Data>
      </Cell>
      <Cell ss:StyleID="Cell">
        <Data ss:Type="Number">
          <xsl:value-of select="L+Le" />
        </Data>
      </Cell>
      <Cell ss:StyleID="Cell">
        <Data ss:Type="Number">
          <xsl:value-of select="D" />
        </Data>
      </Cell>
      <Cell ss:StyleID="Cell">
        <Data ss:Type="Number">
          <xsl:value-of select="v" />
        </Data>
      </Cell>
      <Cell ss:StyleID="Cell">
        <Data ss:Type="Number">
          <xsl:value-of select="R" />
        </Data>
      </Cell>
      <Cell ss:StyleID="Cell">
        <Data ss:Type="Number">
          <xsl:value-of select="H" />
        </Data>
      </Cell>
    </Row>
  </xsl:template>
</xsl:stylesheet>