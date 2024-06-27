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
      <Worksheet ss:Name="ИКС (жилые дома)">
        <Table>
          <Column ss:Width="150" />
          <Column ss:Width="150" />
          <Column ss:Width="150" />
          <!-- <<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<< Столбцы таблицы >>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>> -->
          <Row >
            <Cell ss:StyleID="Column">
              <Data ss:Type="String">Регион</Data>
            </Cell>
            <Cell ss:StyleID="Column">
              <Data ss:Type="String">Населенный пункт</Data>
            </Cell>
            <Cell ss:StyleID="Column">
              <Data ss:Type="String">Адрес</Data>
            </Cell>
          </Row>
          <!-- <<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<< Данные таблицы >>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>> -->
          <xsl:apply-templates select="//GisKvp" />
        </Table>
      </Worksheet>
      <Worksheet ss:Name="Квартплата">
        <Table>
          <Column ss:Width="150" />
          <Column ss:Width="150" />
          <Column ss:Width="150" />
          <!-- <<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<< Столбцы таблицы >>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>> -->
          <Row>
            <Cell ss:StyleID="Column">
              <Data ss:Type="String">Регион</Data>
            </Cell>
            <Cell ss:StyleID="Column">
              <Data ss:Type="String">Населенный пункт</Data>
            </Cell>
            <Cell ss:StyleID="Column">
              <Data ss:Type="String">Адрес</Data>
            </Cell>
          </Row>
          <!-- <<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<< Данные таблицы >>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>> -->
          <xsl:apply-templates select="//Kvp" />
        </Table>
      </Worksheet>
      <Worksheet ss:Name="ИКС (дог. подкл.)">
        <Table>
          <Column ss:Width="150" />
          <Column ss:Width="150" />
          <Column ss:Width="150" />
          <!-- <<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<< Столбцы таблицы >>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>> -->
          <Row>
            <Cell ss:StyleID="Column">
              <Data ss:Type="String">Регион</Data>
            </Cell>
            <Cell ss:StyleID="Column">
              <Data ss:Type="String">Населенный пункт</Data>
            </Cell>
            <Cell ss:StyleID="Column">
              <Data ss:Type="String">Наименование</Data>
            </Cell>
          </Row>
          <!-- <<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<< Данные таблицы >>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>> -->
          <xsl:apply-templates select="//GisJur" />
        </Table>
      </Worksheet>
      <Worksheet ss:Name="Расчеты с юр. лицами">
        <Table>
          <Column ss:Width="150" />
          <Column ss:Width="150" />
          <Column ss:Width="150" />
          <!-- <<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<< Столбцы таблицы >>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>> -->
          <Row>
            <Cell ss:StyleID="Column">
              <Data ss:Type="String">Регион</Data>
            </Cell>
            <Cell ss:StyleID="Column">
              <Data ss:Type="String">Населенный пункт</Data>
            </Cell>
            <Cell ss:StyleID="Column">
              <Data ss:Type="String">Наименование</Data>
            </Cell>
          </Row>
          <!-- <<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<< Данные таблицы >>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>> -->
          <xsl:apply-templates select="//Jur" />
        </Table>
      </Worksheet>
    </Workbook>
  </xsl:template>
  <!-- <<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<< Шаблон данных >>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>> -->
  <xsl:template match="GisKvp">
    <Row>
      <Cell ss:StyleID="Cell">
        <Data ss:Type="String">
          <xsl:value-of select="region" />
        </Data>
      </Cell>
      <Cell ss:StyleID="Cell">
        <Data ss:Type="String">
          <xsl:value-of select="city" />
        </Data>
      </Cell>
      <Cell ss:StyleID="Cell">
        <Data ss:Type="String">
          <xsl:value-of select="addr" />
        </Data>
      </Cell>
    </Row>
  </xsl:template>
  <xsl:template match="Kvp">
    <Row>
      <Cell ss:StyleID="Cell">
        <Data ss:Type="String">
          <xsl:value-of select="region" />
        </Data>
      </Cell>
      <Cell ss:StyleID="Cell">
        <Data ss:Type="String">
          <xsl:value-of select="city" />
        </Data>
      </Cell>
      <Cell ss:StyleID="Cell">
        <Data ss:Type="String">
          <xsl:value-of select="addr" />
        </Data>
      </Cell>
    </Row>
  </xsl:template>
  <xsl:template match="GisJur">
    <Row>
      <Cell ss:StyleID="Cell">
        <Data ss:Type="String">
          <xsl:value-of select="region" />
        </Data>
      </Cell>
      <Cell ss:StyleID="Cell">
        <Data ss:Type="String">
          <xsl:value-of select="city" />
        </Data>
      </Cell>
      <Cell ss:StyleID="Cell">
        <Data ss:Type="String">
          <xsl:value-of select="jur_name" />
        </Data>
      </Cell>
    </Row>
  </xsl:template>
  <xsl:template match="Jur">
    <Row>
      <Cell ss:StyleID="Cell">
        <Data ss:Type="String">
          <xsl:value-of select="region" />
        </Data>
      </Cell>
      <Cell ss:StyleID="Cell">
        <Data ss:Type="String">
          <xsl:value-of select="city" />
        </Data>
      </Cell>
      <Cell ss:StyleID="Cell">
        <Data ss:Type="String">
          <xsl:value-of select="jur_name" />
        </Data>
      </Cell>
    </Row>
  </xsl:template>
</xsl:stylesheet>