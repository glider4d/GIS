<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns="urn:schemas-microsoft-com:office:spreadsheet" xmlns:ss="urn:schemas-microsoft-com:office:spreadsheet" xmlns:x="urn:schemas-microsoft-com:office:excel" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:html="http://www.w3.org/TR/REC-html40">
  <!-- <<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<< Шаблон >>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>> -->
  <xsl:template match="/">
    <xsl:processing-instruction name="mso-application">
      <xsl:text>progid="Excel.Sheet"</xsl:text>
    </xsl:processing-instruction>
    <Workbook>
      <!-- <<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<< Стили шаблона >>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>> -->
      <Styles>
        <!-- <<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<< Общие стили >>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>> -->
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
        <!-- <<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<< Стили листа ТХ1/ТХ3 >>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>> -->
        <Style ss:ID="s63" ss:Name="Обычный 2">
          <Alignment ss:Vertical="Bottom"/>
          <Borders/>
          <Font ss:FontName="Arial Cyr" x:CharSet="204"/>
          <Interior/>
          <NumberFormat/>
          <Protection/>
        </Style>
        <Style ss:ID="s62" ss:Name="Обычный 34">
          <Alignment ss:Vertical="Bottom"/>
          <Borders/>
          <Font ss:FontName="Arial Cyr" x:CharSet="204"/>
          <Interior/>
          <NumberFormat/>
          <Protection/>
        </Style>
        <Style ss:ID="m201520164" ss:Parent="s62">
          <Alignment ss:Horizontal="Center" ss:Vertical="Center" ss:WrapText="1"/>
          <Borders>
            <Border ss:Position="Bottom" ss:LineStyle="Continuous" ss:Weight="1"/>
            <Border ss:Position="Left" ss:LineStyle="Continuous" ss:Weight="1"/>
            <Border ss:Position="Right" ss:LineStyle="Continuous" ss:Weight="1"/>
            <Border ss:Position="Top" ss:LineStyle="Continuous" ss:Weight="1"/>
          </Borders>
        </Style>
        <Style ss:ID="m201520184" ss:Parent="s62">
          <Alignment ss:Horizontal="Center" ss:Vertical="Center" ss:WrapText="1"/>
          <Borders>
            <Border ss:Position="Bottom" ss:LineStyle="Continuous" ss:Weight="1"/>
            <Border ss:Position="Left" ss:LineStyle="Continuous" ss:Weight="1"/>
            <Border ss:Position="Right" ss:LineStyle="Continuous" ss:Weight="1"/>
            <Border ss:Position="Top" ss:LineStyle="Continuous" ss:Weight="1"/>
          </Borders>
        </Style>
        <Style ss:ID="m201519784" ss:Parent="s62">
          <Alignment ss:Horizontal="Center" ss:Vertical="Center" ss:WrapText="1"/>
          <Borders>
            <Border ss:Position="Bottom" ss:LineStyle="Continuous" ss:Weight="1"/>
            <Border ss:Position="Left" ss:LineStyle="Continuous" ss:Weight="1"/>
            <Border ss:Position="Right" ss:LineStyle="Continuous" ss:Weight="1"/>
            <Border ss:Position="Top" ss:LineStyle="Continuous" ss:Weight="1"/>
          </Borders>
          <Interior ss:Color="#FFFF99" ss:Pattern="Solid"/>
          <Protection ss:Protected="0"/>
        </Style>
        <Style ss:ID="m201519804" ss:Parent="s62">
          <Alignment ss:Horizontal="Center" ss:Vertical="Center" ss:WrapText="1"/>
          <Borders>
            <Border ss:Position="Bottom" ss:LineStyle="Continuous" ss:Weight="1"/>
            <Border ss:Position="Left" ss:LineStyle="Continuous" ss:Weight="1"/>
            <Border ss:Position="Right" ss:LineStyle="Continuous" ss:Weight="1"/>
            <Border ss:Position="Top" ss:LineStyle="Continuous" ss:Weight="1"/>
          </Borders>
          <Interior ss:Color="#FFFF99" ss:Pattern="Solid"/>
          <Protection ss:Protected="0"/>
        </Style>
        <Style ss:ID="m201519824" ss:Parent="s62">
          <Alignment ss:Horizontal="Center" ss:Vertical="Center" ss:WrapText="1"/>
          <Borders>
            <Border ss:Position="Bottom" ss:LineStyle="Continuous" ss:Weight="1"/>
            <Border ss:Position="Left" ss:LineStyle="Continuous" ss:Weight="1"/>
            <Border ss:Position="Right" ss:LineStyle="Continuous" ss:Weight="1"/>
            <Border ss:Position="Top" ss:LineStyle="Continuous" ss:Weight="1"/>
          </Borders>
          <Interior ss:Color="#FFFF99" ss:Pattern="Solid"/>
          <Protection ss:Protected="0"/>
        </Style>
        <Style ss:ID="m201519504" ss:Parent="s62">
          <Alignment ss:Horizontal="Center" ss:Vertical="Center" ss:WrapText="1"/>
          <Borders>
            <Border ss:Position="Bottom" ss:LineStyle="Continuous" ss:Weight="1"/>
            <Border ss:Position="Left" ss:LineStyle="Continuous" ss:Weight="1"/>
            <Border ss:Position="Right" ss:LineStyle="Continuous" ss:Weight="1"/>
            <Border ss:Position="Top" ss:LineStyle="Continuous" ss:Weight="1"/>
          </Borders>
          <Font ss:FontName="Arial Cyr" x:CharSet="204"/>
          <Interior ss:Color="#D8E4BC" ss:Pattern="Solid"/>
        </Style>
        <Style ss:ID="m201519524" ss:Parent="s62">
          <Alignment ss:Horizontal="Center" ss:Vertical="Center" ss:WrapText="1"/>
          <Borders>
            <Border ss:Position="Bottom" ss:LineStyle="Continuous" ss:Weight="1"/>
            <Border ss:Position="Left" ss:LineStyle="Continuous" ss:Weight="1"/>
            <Border ss:Position="Right" ss:LineStyle="Continuous" ss:Weight="1"/>
            <Border ss:Position="Top" ss:LineStyle="Continuous" ss:Weight="1"/>
          </Borders>
          <Font ss:FontName="Arial Cyr" x:CharSet="204"/>
          <Interior ss:Color="#D8E4BC" ss:Pattern="Solid"/>
        </Style>
        <Style ss:ID="m201519544" ss:Parent="s62">
          <Alignment ss:Horizontal="Center" ss:Vertical="Center" ss:WrapText="1"/>
          <Borders>
            <Border ss:Position="Bottom" ss:LineStyle="Continuous" ss:Weight="1"/>
            <Border ss:Position="Left" ss:LineStyle="Continuous" ss:Weight="1"/>
            <Border ss:Position="Right" ss:LineStyle="Continuous" ss:Weight="1"/>
            <Border ss:Position="Top" ss:LineStyle="Continuous" ss:Weight="1"/>
          </Borders>
          <Interior ss:Color="#D8E4BC" ss:Pattern="Solid"/>
        </Style>
        <Style ss:ID="m201519664" ss:Parent="s62">
          <Alignment ss:Horizontal="Center" ss:Vertical="Center" ss:WrapText="1"/>
          <Borders>
            <Border ss:Position="Bottom" ss:LineStyle="Continuous" ss:Weight="1"/>
            <Border ss:Position="Left" ss:LineStyle="Continuous" ss:Weight="1"/>
            <Border ss:Position="Right" ss:LineStyle="Continuous" ss:Weight="1"/>
            <Border ss:Position="Top" ss:LineStyle="Continuous" ss:Weight="1"/>
          </Borders>
          <Interior ss:Color="#FFFFFF" ss:Pattern="Solid"/>
          <Protection ss:Protected="0"/>
        </Style>
        <Style ss:ID="m201519684" ss:Parent="s62">
          <Alignment ss:Horizontal="Center" ss:Vertical="Center" ss:WrapText="1"/>
          <Borders>
            <Border ss:Position="Bottom" ss:LineStyle="Continuous" ss:Weight="1"/>
            <Border ss:Position="Left" ss:LineStyle="Continuous" ss:Weight="1"/>
            <Border ss:Position="Right" ss:LineStyle="Continuous" ss:Weight="1"/>
            <Border ss:Position="Top" ss:LineStyle="Continuous" ss:Weight="1"/>
          </Borders>
          <Interior ss:Color="#FFFFFF" ss:Pattern="Solid"/>
          <Protection ss:Protected="0"/>
        </Style>
        <Style ss:ID="m143752700" ss:Parent="s62">
          <Alignment ss:Horizontal="Center" ss:Vertical="Center" ss:WrapText="1"/>
          <Borders>
            <Border ss:Position="Bottom" ss:LineStyle="Continuous" ss:Weight="1"/>
            <Border ss:Position="Left" ss:LineStyle="Continuous" ss:Weight="1"/>
            <Border ss:Position="Right" ss:LineStyle="Continuous" ss:Weight="1"/>
            <Border ss:Position="Top" ss:LineStyle="Continuous" ss:Weight="1"/>
          </Borders>
          <Interior ss:Color="#FFFFFF" ss:Pattern="Solid"/>
          <Protection ss:Protected="0"/>
        </Style>
        <Style ss:ID="s394" ss:Parent="s62">
          <Alignment ss:Horizontal="Center" ss:Vertical="Bottom"/>
          <Font ss:FontName="Arial Cyr" x:CharSet="204"/>
        </Style>
        <Style ss:ID="s395" ss:Parent="s62">
          <Alignment ss:Horizontal="Center" ss:Vertical="Bottom"/>
          <Font ss:FontName="Arial Cyr" x:CharSet="204" ss:Bold="1"/>
        </Style>
        <Style ss:ID="s396" ss:Parent="s62">
          <Alignment ss:Horizontal="Left" ss:Vertical="Bottom"/>
          <Font ss:FontName="Arial Cyr" x:CharSet="204" ss:Bold="1"/>
        </Style>
        <Style ss:ID="s398" ss:Parent="s62">
          <Alignment ss:Horizontal="CenterAcrossSelection" ss:Vertical="Bottom"/>
          <Font ss:FontName="Arial Cyr" x:CharSet="204" ss:Color="#FF0000" ss:Bold="1"/>
        </Style>
        <Style ss:ID="s399" ss:Parent="s62">
          <Alignment ss:Horizontal="CenterAcrossSelection" ss:Vertical="Bottom"/>
          <Font ss:FontName="Arial Cyr" x:CharSet="204"/>
        </Style>
        <Style ss:ID="s400" ss:Parent="s62">
          <Alignment ss:Horizontal="CenterAcrossSelection" ss:Vertical="Bottom"/>
          <Font ss:FontName="Arial Cyr" x:CharSet="204" ss:Bold="1"/>
        </Style>
        <Style ss:ID="s401" ss:Parent="s62">
          <Alignment ss:Horizontal="Right" ss:Vertical="Bottom"/>
          <Font ss:FontName="Arial Cyr" x:CharSet="204" ss:Bold="1"/>
        </Style>
        <Style ss:ID="s402" ss:Parent="s63">
          <Alignment ss:Horizontal="Center" ss:Vertical="Bottom"/>
          <Font ss:FontName="Arial Cyr" x:CharSet="204" ss:Bold="1"/>
          <Protection ss:Protected="0"/>
        </Style>
        <Style ss:ID="s403" ss:Parent="s62">
          <Alignment ss:Horizontal="CenterAcrossSelection" ss:Vertical="Bottom"/>
          <Font ss:FontName="Arial Cyr" x:CharSet="204" ss:Bold="1"/>
          <Protection ss:Protected="0"/>
        </Style>
        <Style ss:ID="s404" ss:Parent="s62">
          <Alignment ss:Horizontal="Center" ss:Vertical="Bottom"/>
        </Style>
        <Style ss:ID="s405" ss:Parent="s62">
          <Alignment ss:Vertical="Top"/>
          <Borders/>
          <Font ss:FontName="Arial Cyr" x:CharSet="204"/>
        </Style>
        <Style ss:ID="s406" ss:Parent="s62">
          <Alignment ss:Horizontal="Center" ss:Vertical="Top"/>
          <Borders/>
          <Font ss:FontName="Arial Cyr" x:CharSet="204"/>
        </Style>
        <Style ss:ID="s407" ss:Parent="s62">
          <Borders/>
          <Font ss:FontName="Arial Cyr" x:CharSet="204"/>
        </Style>
        <Style ss:ID="s409" ss:Parent="s62">
          <Alignment ss:Horizontal="Center" ss:Vertical="Center" ss:WrapText="1"/>
          <Borders>
            <Border ss:Position="Bottom" ss:LineStyle="Continuous" ss:Weight="1"/>
            <Border ss:Position="Left" ss:LineStyle="Continuous" ss:Weight="1"/>
            <Border ss:Position="Right" ss:LineStyle="Continuous" ss:Weight="1"/>
            <Border ss:Position="Top" ss:LineStyle="Continuous" ss:Weight="1"/>
          </Borders>
          <Interior ss:Color="#EBF1DE" ss:Pattern="Solid"/>
          <Protection ss:Protected="0"/>
        </Style>
        <Style ss:ID="s410" ss:Parent="s62">
          <Alignment ss:Horizontal="Center" ss:Vertical="Center" ss:WrapText="1"/>
          <Borders>
            <Border ss:Position="Bottom" ss:LineStyle="Continuous" ss:Weight="1"/>
            <Border ss:Position="Left" ss:LineStyle="Continuous" ss:Weight="1"/>
            <Border ss:Position="Right" ss:LineStyle="Continuous" ss:Weight="1"/>
            <Border ss:Position="Top" ss:LineStyle="Continuous" ss:Weight="1"/>
          </Borders>
          <Interior ss:Color="#FFFFCC" ss:Pattern="Solid"/>
          <Protection ss:Protected="0"/>
        </Style>
        <Style ss:ID="s411" ss:Parent="s62">
          <Alignment ss:Horizontal="Center" ss:Vertical="Center" ss:WrapText="1"/>
          <Borders>
            <Border ss:Position="Bottom" ss:LineStyle="Continuous" ss:Weight="1"/>
            <Border ss:Position="Left" ss:LineStyle="Continuous" ss:Weight="1"/>
            <Border ss:Position="Right" ss:LineStyle="Continuous" ss:Weight="1"/>
            <Border ss:Position="Top" ss:LineStyle="Continuous" ss:Weight="1"/>
          </Borders>
          <Interior/>
          <Protection ss:Protected="0"/>
        </Style>
        <Style ss:ID="s412" ss:Parent="s62">
          <Alignment ss:Horizontal="Center" ss:Vertical="Bottom"/>
          <Borders>
            <Border ss:Position="Bottom" ss:LineStyle="Continuous" ss:Weight="1"/>
            <Border ss:Position="Left" ss:LineStyle="Continuous" ss:Weight="1"/>
            <Border ss:Position="Right" ss:LineStyle="Continuous" ss:Weight="1"/>
            <Border ss:Position="Top" ss:LineStyle="Continuous" ss:Weight="1"/>
          </Borders>
          <Font ss:FontName="Arial Cyr" x:CharSet="204"/>
        </Style>
        <Style ss:ID="s455" ss:Parent="s62">
          <Alignment ss:Horizontal="Center" ss:Vertical="Center" ss:WrapText="1"/>
          <Borders>
            <Border ss:Position="Bottom" ss:LineStyle="Continuous" ss:Weight="1"/>
            <Border ss:Position="Left" ss:LineStyle="Continuous" ss:Weight="1"/>
            <Border ss:Position="Right" ss:LineStyle="Continuous" ss:Weight="1"/>
            <Border ss:Position="Top" ss:LineStyle="Continuous" ss:Weight="1"/>
          </Borders>
          <Interior ss:Color="#FFFF99" ss:Pattern="Solid"/>
          <Protection ss:Protected="0"/>
        </Style>
        <Style ss:ID="s457" ss:Parent="s62">
          <Alignment ss:Horizontal="Center" ss:Vertical="Center"/>
          <Borders>
            <Border ss:Position="Bottom" ss:LineStyle="Continuous" ss:Weight="1"/>
            <Border ss:Position="Left" ss:LineStyle="Continuous" ss:Weight="1"/>
            <Border ss:Position="Right" ss:LineStyle="Continuous" ss:Weight="1"/>
            <Border ss:Position="Top" ss:LineStyle="Continuous" ss:Weight="1"/>
            </Borders>
          <Font ss:FontName="Arial Cyr" x:CharSet="204"/>
          <Interior ss:Color="#FFFF99" ss:Pattern="Solid"/>
          <Protection ss:Protected="0"/>
        </Style>
        <Style ss:ID="s458" ss:Parent="s62">
          <Alignment ss:Horizontal="Center" ss:Vertical="Center"/>
          <Borders>
            <Border ss:Position="Bottom" ss:LineStyle="Continuous" ss:Weight="1"/>
            <Border ss:Position="Left" ss:LineStyle="Continuous" ss:Weight="1"/>
            <Border ss:Position="Right" ss:LineStyle="Continuous" ss:Weight="1"/>
            <Border ss:Position="Top" ss:LineStyle="Continuous" ss:Weight="1"/>
          </Borders>
          <Interior/>
          <Protection ss:Protected="0"/>
        </Style>
        <Style ss:ID="s459" ss:Parent="s62">
          <Alignment ss:Horizontal="Center" ss:Vertical="Center" ss:WrapText="1"/>
          <Borders>
            <Border ss:Position="Bottom" ss:LineStyle="Continuous" ss:Weight="1"/>
            <Border ss:Position="Left" ss:LineStyle="Continuous" ss:Weight="1"/>
            <Border ss:Position="Right" ss:LineStyle="Continuous" ss:Weight="1"/>
            <Border ss:Position="Top" ss:LineStyle="Continuous" ss:Weight="1"/>
          </Borders>
          <Font ss:FontName="Arial Cyr" x:CharSet="204"/>
        </Style>
        <Style ss:ID="s460" ss:Parent="s62">
          <Alignment ss:Horizontal="Center" ss:Vertical="Center" ss:WrapText="1"/>
          <Borders>
            <Border ss:Position="Bottom" ss:LineStyle="Continuous" ss:Weight="1"/>
            <Border ss:Position="Left" ss:LineStyle="Continuous" ss:Weight="1"/>
            <Border ss:Position="Right" ss:LineStyle="Continuous" ss:Weight="1"/>
            <Border ss:Position="Top" ss:LineStyle="Continuous" ss:Weight="1"/>
          </Borders>
          <Interior ss:Color="#F2DCDB" ss:Pattern="Solid"/>
        </Style>
        <Style ss:ID="s466" ss:Parent="s62">
          <Alignment ss:Horizontal="Center" ss:Vertical="Center"/>
          <Borders>
            <Border ss:Position="Bottom" ss:LineStyle="Continuous" ss:Weight="1"/>
            <Border ss:Position="Left" ss:LineStyle="Continuous" ss:Weight="1"/>
            <Border ss:Position="Right" ss:LineStyle="Continuous" ss:Weight="1"/>
            <Border ss:Position="Top" ss:LineStyle="Continuous" ss:Weight="1"/>
          </Borders>
          <Font ss:FontName="Arial Cyr" x:CharSet="204"/>
          <Interior/>
          <Protection ss:Protected="0"/>
        </Style>
        <Style ss:ID="s467" ss:Parent="s62">
          <Alignment ss:Horizontal="Center" ss:Vertical="Center" ss:WrapText="1"/>
          <Borders>
            <Border ss:Position="Bottom" ss:LineStyle="Continuous" ss:Weight="1"/>
            <Border ss:Position="Left" ss:LineStyle="Continuous" ss:Weight="1"/>
            <Border ss:Position="Right" ss:LineStyle="Continuous" ss:Weight="1"/>
            <Border ss:Position="Top" ss:LineStyle="Continuous" ss:Weight="1"/>
          </Borders>
          <Protection ss:Protected="0"/>
        </Style>
        <Style ss:ID="s469" ss:Parent="s62">
          <Alignment ss:Horizontal="Center" ss:Vertical="Center" ss:WrapText="1"/>
          <Borders>
            <Border ss:Position="Bottom" ss:LineStyle="Continuous" ss:Weight="1"/>
            <Border ss:Position="Left" ss:LineStyle="Continuous" ss:Weight="1"/>
            <Border ss:Position="Right" ss:LineStyle="Continuous" ss:Weight="1"/>
            <Border ss:Position="Top" ss:LineStyle="Continuous" ss:Weight="1"/>
          </Borders>
          <Interior ss:Color="#E6B8B7" ss:Pattern="Solid"/>
        </Style>
        <Style ss:ID="s470" ss:Parent="s62">
          <Alignment ss:Horizontal="Center" ss:Vertical="Center" ss:WrapText="1"/>
          <Borders>
            <Border ss:Position="Bottom" ss:LineStyle="Continuous" ss:Weight="1"/>
            <Border ss:Position="Left" ss:LineStyle="Continuous" ss:Weight="1"/>
            <Border ss:Position="Right" ss:LineStyle="Continuous" ss:Weight="1"/>
            <Border ss:Position="Top" ss:LineStyle="Continuous" ss:Weight="1"/>
          </Borders>
          <Font ss:FontName="Arial Cyr" x:CharSet="204"/>
          <Interior ss:Color="#D8E4BC" ss:Pattern="Solid"/>
        </Style>
        <Style ss:ID="s475" ss:Parent="s62">
          <Alignment ss:Horizontal="Center" ss:Vertical="Center" ss:WrapText="1"/>
          <Borders>
            <Border ss:Position="Bottom" ss:LineStyle="Continuous" ss:Weight="1"/>
            <Border ss:Position="Left" ss:LineStyle="Continuous" ss:Weight="1"/>
            <Border ss:Position="Right" ss:LineStyle="Continuous" ss:Weight="1"/>
            <Border ss:Position="Top" ss:LineStyle="Continuous" ss:Weight="1"/>
          </Borders>
          <Interior ss:Color="#D8E4BC" ss:Pattern="Solid"/>
          <Protection ss:Protected="0"/>
        </Style>
        <Style ss:ID="s477" ss:Parent="s62">
          <Alignment ss:Horizontal="Center" ss:Vertical="Bottom"/>
          <Borders>
            <Border ss:Position="Bottom" ss:LineStyle="Continuous" ss:Weight="1"/>
            <Border ss:Position="Left" ss:LineStyle="Continuous" ss:Weight="1"/>
            <Border ss:Position="Right" ss:LineStyle="Continuous" ss:Weight="1"/>
            <Border ss:Position="Top" ss:LineStyle="Continuous" ss:Weight="1"/>
          </Borders>
          <Font ss:FontName="Arial Cyr" x:CharSet="204"/>
          <Interior ss:Color="#D8E4BC" ss:Pattern="Solid"/>
          <Protection ss:Protected="0"/>
        </Style>
        <Style ss:ID="s478" ss:Parent="s62">
          <Alignment ss:Horizontal="Center" ss:Vertical="Center"/>
          <Borders>
            <Border ss:Position="Bottom" ss:LineStyle="Continuous" ss:Weight="1"/>
            <Border ss:Position="Left" ss:LineStyle="Continuous" ss:Weight="1"/>
            <Border ss:Position="Right" ss:LineStyle="Continuous" ss:Weight="1"/>
            <Border ss:Position="Top" ss:LineStyle="Continuous" ss:Weight="1"/>
          </Borders>
          <Font ss:FontName="Arial Cyr" x:CharSet="204" x:Family="Swiss" ss:Size="9"/>
          <Interior/>
        </Style>
        <Style ss:ID="s479" ss:Parent="s62">
          <Alignment ss:Horizontal="Center" ss:Vertical="Center" ss:WrapText="1"/>
          <Borders>
            <Border ss:Position="Bottom" ss:LineStyle="Continuous" ss:Weight="1"/>
            <Border ss:Position="Left" ss:LineStyle="Continuous" ss:Weight="1"/>
            <Border ss:Position="Right" ss:LineStyle="Continuous" ss:Weight="1"/>
            <Border ss:Position="Top" ss:LineStyle="Continuous" ss:Weight="1"/>
          </Borders>
          <Font ss:FontName="Arial Cyr" x:CharSet="204"/>
          <Interior/>
        </Style>
        <Style ss:ID="s480" ss:Parent="s62">
          <Alignment ss:Horizontal="Center" ss:Vertical="Center" ss:WrapText="1"/>
          <Borders>
            <Border ss:Position="Bottom" ss:LineStyle="Continuous" ss:Weight="1"/>
            <Border ss:Position="Left" ss:LineStyle="Continuous" ss:Weight="1"/>
            <Border ss:Position="Right" ss:LineStyle="Continuous" ss:Weight="1"/>
            <Border ss:Position="Top" ss:LineStyle="Continuous" ss:Weight="1"/>
          </Borders>
          <Interior/>
        </Style>
        <Style ss:ID="s482" ss:Parent="s62">
          <Alignment ss:Horizontal="Center" ss:Vertical="Center"/>
          <Borders>
            <Border ss:Position="Bottom" ss:LineStyle="Continuous" ss:Weight="1"/>
            <Border ss:Position="Left" ss:LineStyle="Continuous" ss:Weight="1"/>
            <Border ss:Position="Right" ss:LineStyle="Continuous" ss:Weight="1"/>
            <Border ss:Position="Top" ss:LineStyle="Continuous" ss:Weight="1"/>
          </Borders>
          <Font ss:FontName="Arial Cyr" x:CharSet="204" x:Family="Swiss" ss:Size="9"/>
          <Interior ss:Color="#92D050" ss:Pattern="Solid"/>
        </Style>
        <Style ss:ID="s483" ss:Parent="s62">
          <Alignment ss:Horizontal="Center" ss:Vertical="Center"/>
          <Borders>
            <Border ss:Position="Bottom" ss:LineStyle="Continuous" ss:Weight="1"/>
            <Border ss:Position="Left" ss:LineStyle="Continuous" ss:Weight="1"/>
            <Border ss:Position="Right" ss:LineStyle="Continuous" ss:Weight="1"/>
            <Border ss:Position="Top" ss:LineStyle="Continuous" ss:Weight="1"/>
          </Borders>
          <Font ss:FontName="Arial Cyr" x:CharSet="204" x:Family="Swiss" ss:Size="9"/>
          <Interior ss:Color="#FFFF00" ss:Pattern="Solid"/>
        </Style>
        <Style ss:ID="s487" ss:Parent="s62">
          <Alignment ss:Horizontal="Center" ss:Vertical="Center"/>
          <Borders>
            <Border ss:Position="Bottom" ss:LineStyle="Continuous" ss:Weight="1"/>
            <Border ss:Position="Left" ss:LineStyle="Continuous" ss:Weight="1"/>
            <Border ss:Position="Right" ss:LineStyle="Continuous" ss:Weight="1"/>
            <Border ss:Position="Top" ss:LineStyle="Continuous" ss:Weight="1"/>
          </Borders>
          <Interior ss:Color="#F2DCDB" ss:Pattern="Solid"/>
          <Protection ss:Protected="0"/>
        </Style>
        <Style ss:ID="s488" ss:Parent="s62">
          <Alignment ss:Horizontal="Center" ss:Vertical="Center"/>
          <Borders>
            <Border ss:Position="Bottom" ss:LineStyle="Continuous" ss:Weight="1"/>
            <Border ss:Position="Left" ss:LineStyle="Continuous" ss:Weight="1"/>
            <Border ss:Position="Right" ss:LineStyle="Continuous" ss:Weight="1"/>
            <Border ss:Position="Top" ss:LineStyle="Continuous" ss:Weight="1"/>
          </Borders>
          <Interior ss:Color="#D8E4BC" ss:Pattern="Solid"/>
          <Protection ss:Protected="0"/>
        </Style>
        <Style ss:ID="s489" ss:Parent="s62">
          <Alignment ss:Horizontal="Center" ss:Vertical="Center"/>
          <Borders>
            <Border ss:Position="Bottom" ss:LineStyle="Continuous" ss:Weight="1"/>
            <Border ss:Position="Left" ss:LineStyle="Continuous" ss:Weight="1"/>
            <Border ss:Position="Right" ss:LineStyle="Continuous" ss:Weight="1"/>
            <Border ss:Position="Top" ss:LineStyle="Continuous" ss:Weight="1"/>
          </Borders>
          <Interior ss:Color="#FFFF99" ss:Pattern="Solid"/>
          <Protection ss:Protected="0"/>
        </Style>
        <Style ss:ID="s490" ss:Parent="s62">
          <Alignment ss:Horizontal="Center" ss:Vertical="Center" ss:WrapText="1"/>
          <Borders>
            <Border ss:Position="Bottom" ss:LineStyle="Continuous" ss:Weight="1"/>
            <Border ss:Position="Left" ss:LineStyle="Continuous" ss:Weight="1"/>
            <Border ss:Position="Right" ss:LineStyle="Continuous" ss:Weight="1"/>
            <Border ss:Position="Top" ss:LineStyle="Continuous" ss:Weight="1"/>
          </Borders>
        </Style>
        <Style ss:ID="s293a">
          <Borders/>
          <Font ss:FontName="Arial Cyr" x:CharSet="204"/>
          <Interior/>
        </Style>
        <Style ss:ID="s294a">
          <Alignment ss:Horizontal="Center" ss:Vertical="Bottom"/>
          <Borders>
            <Border ss:Position="Bottom" ss:LineStyle="Continuous" ss:Weight="1"/>
            <Border ss:Position="Left" ss:LineStyle="Continuous" ss:Weight="1"/>
            <Border ss:Position="Right" ss:LineStyle="Continuous" ss:Weight="1"/>
            <Border ss:Position="Top" ss:LineStyle="Continuous" ss:Weight="1"/>
          </Borders>
          <Font ss:FontName="Arial Cyr" x:CharSet="204" ss:Bold="1"/>
          <Interior ss:Color="#FFCC99" ss:Pattern="Solid"/>
        </Style>
        <Style ss:ID="s295a">
          <Borders>
            <Border ss:Position="Bottom" ss:LineStyle="Continuous" ss:Weight="1"/>
            <Border ss:Position="Left" ss:LineStyle="Continuous" ss:Weight="1"/>
            <Border ss:Position="Right" ss:LineStyle="Continuous" ss:Weight="1"/>
            <Border ss:Position="Top" ss:LineStyle="Continuous" ss:Weight="1"/>
          </Borders>
          <Font ss:FontName="Arial Cyr" x:CharSet="204" ss:Bold="1"/>
          <Interior ss:Color="#FFCC99" ss:Pattern="Solid"/>
        </Style>
        <Style ss:ID="s296a">
          <Alignment ss:Horizontal="Center" ss:Vertical="Bottom"/>
          <Borders>
            <Border ss:Position="Bottom" ss:LineStyle="Continuous" ss:Weight="1"/>
            <Border ss:Position="Left" ss:LineStyle="Continuous" ss:Weight="1"/>
            <Border ss:Position="Right" ss:LineStyle="Continuous" ss:Weight="1"/>
            <Border ss:Position="Top" ss:LineStyle="Continuous" ss:Weight="1"/>
          </Borders>
          <Font ss:FontName="Arial Cyr" x:CharSet="204" ss:Bold="1"/>
          <Interior ss:Color="#99CCFF" ss:Pattern="Solid"/>
        </Style>
        <Style ss:ID="s297a">
          <Borders>
            <Border ss:Position="Bottom" ss:LineStyle="Continuous" ss:Weight="1"/>
            <Border ss:Position="Left" ss:LineStyle="Continuous" ss:Weight="1"/>
            <Border ss:Position="Right" ss:LineStyle="Continuous" ss:Weight="1"/>
            <Border ss:Position="Top" ss:LineStyle="Continuous" ss:Weight="1"/>
          </Borders>
          <Font ss:FontName="Arial Cyr" x:CharSet="204" ss:Bold="1"/>
          <Interior ss:Color="#99CCFF" ss:Pattern="Solid"/>
        </Style>
        <Style ss:ID="s298a">
          <Alignment ss:Horizontal="Center" ss:Vertical="Bottom"/>
          <Borders>
            <Border ss:Position="Bottom" ss:LineStyle="Continuous" ss:Weight="1"/>
            <Border ss:Position="Left" ss:LineStyle="Continuous" ss:Weight="1"/>
            <Border ss:Position="Right" ss:LineStyle="Continuous" ss:Weight="1"/>
            <Border ss:Position="Top" ss:LineStyle="Continuous" ss:Weight="1"/>
          </Borders>
          <Font ss:FontName="Arial Cyr" x:CharSet="204" ss:Bold="1"/>
          <Interior ss:Color="#CCFFFF" ss:Pattern="Solid"/>
        </Style>
        <Style ss:ID="s299a">
          <Borders>
            <Border ss:Position="Bottom" ss:LineStyle="Continuous" ss:Weight="1"/>
            <Border ss:Position="Left" ss:LineStyle="Continuous" ss:Weight="1"/>
            <Border ss:Position="Right" ss:LineStyle="Continuous" ss:Weight="1"/>
            <Border ss:Position="Top" ss:LineStyle="Continuous" ss:Weight="1"/>
          </Borders>
          <Font ss:FontName="Arial Cyr" x:CharSet="204" ss:Bold="1"/>
          <Interior ss:Color="#CCFFFF" ss:Pattern="Solid"/>
        </Style>
        <Style ss:ID="s300a">
          <Font ss:FontName="Calibri" x:CharSet="204" x:Family="Swiss" ss:Size="11"
          ss:Color="#000000"/>
        </Style>
        <Style ss:ID="s301a">
          <Alignment ss:Horizontal="Center" ss:Vertical="Bottom"/>
          <Borders>
            <Border ss:Position="Bottom" ss:LineStyle="Continuous" ss:Weight="1"/>
            <Border ss:Position="Left" ss:LineStyle="Continuous" ss:Weight="1"/>
            <Border ss:Position="Right" ss:LineStyle="Continuous" ss:Weight="1"/>
            <Border ss:Position="Top" ss:LineStyle="Continuous" ss:Weight="1"/>
          </Borders>
          <Font ss:FontName="Calibri" x:CharSet="204" x:Family="Swiss" ss:Size="11"
          ss:Color="#000000"/>
        </Style>
        <Style ss:ID="s302a">
          <Borders>
            <Border ss:Position="Bottom" ss:LineStyle="Continuous" ss:Weight="1"/>
            <Border ss:Position="Left" ss:LineStyle="Continuous" ss:Weight="1"/>
            <Border ss:Position="Right" ss:LineStyle="Continuous" ss:Weight="1"/>
            <Border ss:Position="Top" ss:LineStyle="Continuous" ss:Weight="1"/>
          </Borders>
          <Font ss:FontName="Calibri" x:CharSet="204" x:Family="Swiss" ss:Size="11"
          ss:Color="#000000"/>
        </Style>
        <!-- <<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<< Стили листа ТХ2 >>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>> -->
        <Style ss:ID="m166230568">
          <Alignment ss:Horizontal="Center" ss:Vertical="Center" ss:WrapText="1" />
          <Borders>
            <Border ss:Position="Bottom" ss:LineStyle="Continuous" ss:Weight="1" ss:Color="#808080" />
            <Border ss:Position="Left" ss:LineStyle="Continuous" ss:Weight="1" ss:Color="#808080" />
            <Border ss:Position="Top" ss:LineStyle="Continuous" ss:Weight="1" ss:Color="#808080" />
          </Borders>
          <Font ss:FontName="Calibri" x:CharSet="204" x:Family="Swiss" ss:Size="11" ss:Color="#000000" ss:Bold="1" />
          <Interior ss:Color="#F2DCDB" ss:Pattern="Solid" />
        </Style>
        <Style ss:ID="m166230608">
          <Alignment ss:Horizontal="Center" ss:Vertical="Center" ss:WrapText="1" />
          <Borders>
            <Border ss:Position="Bottom" ss:LineStyle="Continuous" ss:Weight="1" />
            <Border ss:Position="Left" ss:LineStyle="Continuous" ss:Weight="1" />
            <Border ss:Position="Right" ss:LineStyle="Continuous" ss:Weight="1" />
            <Border ss:Position="Top" ss:LineStyle="Continuous" ss:Weight="1" />
          </Borders>
          <Font ss:FontName="Calibri" x:CharSet="204" x:Family="Swiss" ss:Size="11" ss:Color="#000000" ss:Bold="1" />
          <Interior ss:Color="#F2DCDB" ss:Pattern="Solid" />
        </Style>
        <Style ss:ID="s19617">
          <Alignment ss:Horizontal="Center" ss:Vertical="Center" ss:WrapText="1" />
          <Borders>
            <Border ss:Position="Bottom" ss:LineStyle="Continuous" ss:Weight="1" ss:Color="#808080" />
            <Border ss:Position="Left" ss:LineStyle="Continuous" ss:Weight="1" ss:Color="#808080" />
            <Border ss:Position="Right" ss:LineStyle="Continuous" ss:Weight="1" ss:Color="#808080" />
          </Borders>
          <Font ss:FontName="Calibri" x:CharSet="204" x:Family="Swiss" ss:Size="11" ss:Bold="1" />
          <Interior />
          <NumberFormat ss:Format="Standard" />
        </Style>
        <Style ss:ID="s19618">
          <Alignment ss:Horizontal="Center" ss:Vertical="Center" />
          <Borders>
            <Border ss:Position="Bottom" ss:LineStyle="Continuous" ss:Weight="1" ss:Color="#808080" />
            <Border ss:Position="Left" ss:LineStyle="Continuous" ss:Weight="1" ss:Color="#808080" />
            <Border ss:Position="Right" ss:LineStyle="Continuous" ss:Weight="1" ss:Color="#808080" />
          </Borders>
          <Font ss:FontName="Calibri" x:CharSet="204" x:Family="Swiss" ss:Size="11" ss:Bold="1" />
          <Interior />
          <NumberFormat ss:Format="Standard" />
        </Style>
        <Style ss:ID="s19619">
          <Alignment ss:Horizontal="Center" ss:Vertical="Center" ss:WrapText="1" />
          <Borders>
            <Border ss:Position="Bottom" ss:LineStyle="Continuous" ss:Weight="1" ss:Color="#808080" />
            <Border ss:Position="Left" ss:LineStyle="Continuous" ss:Weight="1" ss:Color="#808080" />
            <Border ss:Position="Right" ss:LineStyle="Continuous" ss:Weight="1" ss:Color="#808080" />
          </Borders>
          <Font ss:FontName="Calibri" x:CharSet="204" x:Family="Swiss" ss:Size="11" ss:Bold="1" />
          <Interior ss:Color="#FFFFFF" ss:Pattern="Solid" />
          <NumberFormat ss:Format="Standard" />
        </Style>
        <Style ss:ID="s19620">
          <Alignment ss:Horizontal="Center" ss:Vertical="Center" ss:WrapText="1" />
          <Borders>
            <Border ss:Position="Bottom" ss:LineStyle="Continuous" ss:Weight="1" ss:Color="#808080" />
            <Border ss:Position="Left" ss:LineStyle="Continuous" ss:Weight="1" ss:Color="#808080" />
            <Border ss:Position="Right" ss:LineStyle="Continuous" ss:Weight="1" ss:Color="#808080" />
          </Borders>
          <Font ss:FontName="Calibri" x:CharSet="204" x:Family="Swiss" ss:Size="11" ss:Bold="1" />
          <Interior ss:Color="#FFFFFF" ss:Pattern="Solid" />
          <NumberFormat ss:Format="Fixed" />
        </Style>
        <Style ss:ID="s19621">
          <Alignment ss:Horizontal="Center" ss:Vertical="Center" ss:WrapText="1" />
          <Borders>
            <Border ss:Position="Bottom" ss:LineStyle="Continuous" ss:Weight="1" ss:Color="#808080" />
            <Border ss:Position="Left" ss:LineStyle="Continuous" ss:Weight="1" ss:Color="#808080" />
          </Borders>
          <Font ss:FontName="Calibri" x:CharSet="204" x:Family="Swiss" ss:Size="11" ss:Bold="1" />
          <Interior />
          <NumberFormat ss:Format="Standard" />
        </Style>
        <Style ss:ID="s19624">
          <Alignment ss:Horizontal="Center" ss:Vertical="Center" ss:WrapText="1" />
          <Borders>
            <Border ss:Position="Bottom" ss:LineStyle="Continuous" ss:Weight="1" ss:Color="#808080" />
            <Border ss:Position="Left" ss:LineStyle="Continuous" ss:Weight="1" ss:Color="#808080" />
            <Border ss:Position="Right" ss:LineStyle="Continuous" ss:Weight="1" ss:Color="#808080" />
            <Border ss:Position="Top" ss:LineStyle="Continuous" ss:Weight="1" ss:Color="#808080" />
          </Borders>
          <Font ss:FontName="Calibri" x:CharSet="204" x:Family="Swiss" ss:Size="11" ss:Color="#000000" ss:Bold="1" />
          <Interior ss:Color="#F2DCDB" ss:Pattern="Solid" />
        </Style>
        <Style ss:ID="s19686">
          <Alignment ss:Horizontal="Center" ss:Vertical="Center" ss:WrapText="1" />
          <Borders>
            <Border ss:Position="Bottom" ss:LineStyle="Continuous" ss:Weight="1" />
            <Border ss:Position="Left" ss:LineStyle="Continuous" ss:Weight="1" />
            <Border ss:Position="Right" ss:LineStyle="Continuous" ss:Weight="1" />
          </Borders>
          <Font ss:FontName="Calibri" x:CharSet="204" x:Family="Swiss" ss:Size="11" ss:Bold="1" />
          <Interior />
          <NumberFormat ss:Format="Standard" />
        </Style>
        <Style ss:ID="s91">
          <Borders>
            <Border ss:Position="Bottom" ss:LineStyle="Continuous" ss:Weight="1" />
            <Border ss:Position="Left" ss:LineStyle="Continuous" ss:Weight="1" />
            <Border ss:Position="Right" ss:LineStyle="Continuous" ss:Weight="1" />
            <Border ss:Position="Top" ss:LineStyle="Continuous" ss:Weight="1" />
          </Borders>
        </Style>
        <!-- <<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<< Стили листа ТХ5 >>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>> -->
        <Style ss:ID="s39228">
          <Alignment ss:Horizontal="Left" ss:Vertical="Bottom"/>
          <Borders>
            <Border ss:Position="Bottom" ss:LineStyle="Continuous" ss:Weight="1"
             ss:Color="#BFBFBF"/>
            <Border ss:Position="Left" ss:LineStyle="Continuous" ss:Weight="1"
             ss:Color="#808080"/>
            <Border ss:Position="Right" ss:LineStyle="Continuous" ss:Weight="1"
             ss:Color="#808080"/>
            <Border ss:Position="Top" ss:LineStyle="Continuous" ss:Weight="1"
             ss:Color="#BFBFBF"/>
          </Borders>
          <Font ss:FontName="Arimo" x:CharSet="204" x:Family="Swiss" ss:Size="11"/>
          <Interior ss:Color="#FFFFFF" ss:Pattern="Solid"/>
          <NumberFormat ss:Format="#,##0.000"/>
        </Style>
        <Style ss:ID="s39229">
          <Alignment ss:Horizontal="Left" ss:Vertical="Bottom" ss:Indent="1"/>
          <Borders>
            <Border ss:Position="Bottom" ss:LineStyle="Continuous" ss:Weight="1"
             ss:Color="#BFBFBF"/>
            <Border ss:Position="Left" ss:LineStyle="Continuous" ss:Weight="1"
             ss:Color="#808080"/>
            <Border ss:Position="Right" ss:LineStyle="Continuous" ss:Weight="1"
             ss:Color="#808080"/>
            <Border ss:Position="Top" ss:LineStyle="Continuous" ss:Weight="1"
             ss:Color="#BFBFBF"/>
          </Borders>
          <Font ss:FontName="Arimo" x:CharSet="204" x:Family="Swiss" ss:Size="11"/>
          <Interior ss:Color="#FFFFFF" ss:Pattern="Solid"/>
          <NumberFormat ss:Format="#,##0.000"/>
        </Style>
        <Style ss:ID="s39230">
          <Alignment ss:Horizontal="Center" ss:Vertical="Bottom"/>
          <Borders>
            <Border ss:Position="Bottom" ss:LineStyle="Continuous" ss:Weight="1"
             ss:Color="#BFBFBF"/>
            <Border ss:Position="Left" ss:LineStyle="Continuous" ss:Weight="1"
             ss:Color="#808080"/>
            <Border ss:Position="Right" ss:LineStyle="Continuous" ss:Weight="1"
             ss:Color="#808080"/>
            <Border ss:Position="Top" ss:LineStyle="Continuous" ss:Weight="1"
             ss:Color="#BFBFBF"/>
          </Borders>
          <Font ss:FontName="Arimo" x:CharSet="204" x:Family="Swiss" ss:Size="11"/>
          <Interior ss:Color="#FFFFFF" ss:Pattern="Solid"/>
          <NumberFormat ss:Format="#,##0.000"/>
        </Style>
        <Style ss:ID="s39231">
          <Alignment ss:Horizontal="Left" ss:Vertical="Bottom"/>
          <Borders>
            <Border ss:Position="Bottom" ss:LineStyle="Continuous" ss:Weight="1"
             ss:Color="#BFBFBF"/>
            <Border ss:Position="Left" ss:LineStyle="Continuous" ss:Weight="1"
             ss:Color="#808080"/>
            <Border ss:Position="Right" ss:LineStyle="Continuous" ss:Weight="1"
             ss:Color="#808080"/>
            <Border ss:Position="Top" ss:LineStyle="Continuous" ss:Weight="1"
             ss:Color="#BFBFBF"/>
          </Borders>
          <Font ss:FontName="Arimo" x:CharSet="204" x:Family="Swiss" ss:Size="11"/>
          <Interior ss:Color="#FFFFFF" ss:Pattern="Solid"/>
          <NumberFormat ss:Format="#,##0"/>
        </Style>
        <Style ss:ID="s39232">
          <Alignment ss:Horizontal="Left" ss:Vertical="Bottom" ss:Indent="1"/>
          <Borders>
            <Border ss:Position="Bottom" ss:LineStyle="Continuous" ss:Weight="1"
             ss:Color="#BFBFBF"/>
            <Border ss:Position="Left" ss:LineStyle="Continuous" ss:Weight="1"
             ss:Color="#808080"/>
            <Border ss:Position="Right" ss:LineStyle="Continuous" ss:Weight="1"
             ss:Color="#808080"/>
            <Border ss:Position="Top" ss:LineStyle="Continuous" ss:Weight="1"
             ss:Color="#BFBFBF"/>
          </Borders>
          <Font ss:FontName="Arimo" x:CharSet="204" x:Family="Swiss" ss:Size="11"/>
          <Interior ss:Color="#FFFFFF" ss:Pattern="Solid"/>
          <NumberFormat ss:Format="#,##0"/>
        </Style>
        <Style ss:ID="s39234">
          <Alignment ss:Horizontal="Left" ss:Vertical="Bottom"/>
          <Borders>
            <Border ss:Position="Bottom" ss:LineStyle="Continuous" ss:Weight="1"
             ss:Color="#BFBFBF"/>
            <Border ss:Position="Left" ss:LineStyle="Continuous" ss:Weight="1"
             ss:Color="#808080"/>
            <Border ss:Position="Right" ss:LineStyle="Continuous" ss:Weight="1"
             ss:Color="#808080"/>
            <Border ss:Position="Top" ss:LineStyle="Continuous" ss:Weight="1"
             ss:Color="#BFBFBF"/>
          </Borders>
          <Font ss:FontName="Arimo" x:CharSet="204" x:Family="Swiss" ss:Size="11"
           ss:Bold="1"/>
          <Interior ss:Color="#FFFFFF" ss:Pattern="Solid"/>
          <NumberFormat ss:Format="#,##0"/>
        </Style>
        <Style ss:ID="s39236">
          <Alignment ss:Horizontal="Left" ss:Vertical="Center"/>
          <Borders>
            <Border ss:Position="Bottom" ss:LineStyle="Continuous" ss:Weight="1"
             ss:Color="#BFBFBF"/>
            <Border ss:Position="Left" ss:LineStyle="Continuous" ss:Weight="1"
             ss:Color="#808080"/>
            <Border ss:Position="Right" ss:LineStyle="Continuous" ss:Weight="1"
             ss:Color="#808080"/>
            <Border ss:Position="Top" ss:LineStyle="Continuous" ss:Weight="1"
             ss:Color="#BFBFBF"/>
          </Borders>
          <Font ss:FontName="Arimo" x:CharSet="204" x:Family="Swiss" ss:Size="11"
           ss:Bold="1"/>
          <Interior ss:Color="#FFFFFF" ss:Pattern="Solid"/>
        </Style>
        <Style ss:ID="s39239">
          <Alignment ss:Horizontal="Left" ss:Vertical="Bottom"/>
          <Borders>
            <Border ss:Position="Bottom" ss:LineStyle="Continuous" ss:Weight="1"/>
            <Border ss:Position="Left" ss:LineStyle="Continuous" ss:Weight="1"
             ss:Color="#808080"/>
            <Border ss:Position="Right" ss:LineStyle="Continuous" ss:Weight="1"
             ss:Color="#808080"/>
            <Border ss:Position="Top" ss:LineStyle="Continuous" ss:Weight="1"
             ss:Color="#BFBFBF"/>
          </Borders>
          <Font ss:FontName="Arimo" x:CharSet="204" x:Family="Swiss" ss:Size="11"
           ss:Bold="1"/>
          <Interior ss:Color="#FFFFFF" ss:Pattern="Solid"/>
        </Style>
        <Style ss:ID="s39240">
          <Alignment ss:Horizontal="Center" ss:Vertical="Bottom"/>
          <Borders>
            <Border ss:Position="Bottom" ss:LineStyle="Continuous" ss:Weight="1"/>
            <Border ss:Position="Left" ss:LineStyle="Continuous" ss:Weight="1"
             ss:Color="#808080"/>
            <Border ss:Position="Right" ss:LineStyle="Continuous" ss:Weight="1"
             ss:Color="#808080"/>
            <Border ss:Position="Top" ss:LineStyle="Continuous" ss:Weight="1"
             ss:Color="#BFBFBF"/>
          </Borders>
          <Font ss:FontName="Arimo" x:CharSet="204" x:Family="Swiss" ss:Size="11"
           ss:Bold="1"/>
          <Interior ss:Color="#FFFFFF" ss:Pattern="Solid"/>
        </Style>
        <Style ss:ID="s39294">
          <Alignment ss:Horizontal="Left" ss:Vertical="Bottom" ss:Indent="4"/>
          <Borders>
            <Border ss:Position="Bottom" ss:LineStyle="Continuous" ss:Weight="1"
             ss:Color="#BFBFBF"/>
            <Border ss:Position="Left" ss:LineStyle="Continuous" ss:Weight="1"
             ss:Color="#808080"/>
            <Border ss:Position="Right" ss:LineStyle="Continuous" ss:Weight="1"
             ss:Color="#808080"/>
            <Border ss:Position="Top" ss:LineStyle="Continuous" ss:Weight="1"
             ss:Color="#BFBFBF"/>
          </Borders>
          <Font ss:FontName="Arimo" x:CharSet="204" x:Family="Swiss" ss:Size="11"/>
          <Interior ss:Color="#FFFFFF" ss:Pattern="Solid"/>
          <NumberFormat ss:Format="@"/>
        </Style>
        <Style ss:ID="s39295">
          <Alignment ss:Horizontal="Left" ss:Vertical="Bottom" ss:Indent="4"/>
          <Borders>
            <Border ss:Position="Bottom" ss:LineStyle="Continuous" ss:Weight="1"
             ss:Color="#BFBFBF"/>
            <Border ss:Position="Left" ss:LineStyle="Continuous" ss:Weight="1"
             ss:Color="#808080"/>
            <Border ss:Position="Right" ss:LineStyle="Continuous" ss:Weight="1"
             ss:Color="#808080"/>
            <Border ss:Position="Top" ss:LineStyle="Continuous" ss:Weight="1"
             ss:Color="#BFBFBF"/>
          </Borders>
          <Font ss:FontName="Arimo" x:CharSet="204" x:Family="Swiss" ss:Size="11"/>
          <Interior ss:Color="#FFFFFF" ss:Pattern="Solid"/>
          <NumberFormat ss:Format="Standard"/>
        </Style>
        <Style ss:ID="s39296">
          <Alignment ss:Horizontal="Left" ss:Vertical="Bottom" ss:Indent="3"/>
          <Borders>
            <Border ss:Position="Bottom" ss:LineStyle="Continuous" ss:Weight="1"
             ss:Color="#BFBFBF"/>
            <Border ss:Position="Left" ss:LineStyle="Continuous" ss:Weight="1"
             ss:Color="#808080"/>
            <Border ss:Position="Right" ss:LineStyle="Continuous" ss:Weight="1"
             ss:Color="#808080"/>
            <Border ss:Position="Top" ss:LineStyle="Continuous" ss:Weight="1"
             ss:Color="#BFBFBF"/>
          </Borders>
          <Font ss:FontName="Arimo" x:CharSet="204" x:Family="Swiss" ss:Size="11"
           ss:Bold="1"/>
          <Interior ss:Color="#FFFFFF" ss:Pattern="Solid"/>
          <NumberFormat ss:Format="Standard"/>
        </Style>
        <Style ss:ID="s39298">
          <Alignment ss:Horizontal="Left" ss:Vertical="Bottom"/>
          <Borders>
            <Border ss:Position="Bottom" ss:LineStyle="Continuous" ss:Weight="1"
             ss:Color="#BFBFBF"/>
            <Border ss:Position="Left" ss:LineStyle="Continuous" ss:Weight="1"
             ss:Color="#808080"/>
            <Border ss:Position="Right" ss:LineStyle="Continuous" ss:Weight="1"
             ss:Color="#808080"/>
            <Border ss:Position="Top" ss:LineStyle="Continuous" ss:Weight="1"
             ss:Color="#BFBFBF"/>
          </Borders>
          <Font ss:FontName="Arimo" x:CharSet="204" x:Family="Swiss" ss:Size="11"/>
          <Interior ss:Color="#FFFFFF" ss:Pattern="Solid"/>
          <NumberFormat ss:Format="Standard"/>
        </Style>
        <Style ss:ID="s39299">
          <Alignment ss:Horizontal="Left" ss:Vertical="Bottom" ss:Indent="3"/>
          <Borders>
            <Border ss:Position="Bottom" ss:LineStyle="Continuous" ss:Weight="1"
             ss:Color="#BFBFBF"/>
            <Border ss:Position="Left" ss:LineStyle="Continuous" ss:Weight="1"
             ss:Color="#808080"/>
            <Border ss:Position="Right" ss:LineStyle="Continuous" ss:Weight="1"
             ss:Color="#808080"/>
            <Border ss:Position="Top" ss:LineStyle="Continuous" ss:Weight="1"
             ss:Color="#BFBFBF"/>
          </Borders>
          <Font ss:FontName="Arimo" x:CharSet="204" x:Family="Swiss" ss:Size="11"/>
          <Interior ss:Color="#FFFFFF" ss:Pattern="Solid"/>
          <NumberFormat ss:Format="Standard"/>
        </Style>
        <Style ss:ID="s39300">
          <Alignment ss:Horizontal="Center" ss:Vertical="Bottom"/>
          <Borders>
            <Border ss:Position="Bottom" ss:LineStyle="Continuous" ss:Weight="1"
             ss:Color="#BFBFBF"/>
            <Border ss:Position="Left" ss:LineStyle="Continuous" ss:Weight="1"
             ss:Color="#808080"/>
            <Border ss:Position="Right" ss:LineStyle="Continuous" ss:Weight="1"
             ss:Color="#808080"/>
            <Border ss:Position="Top" ss:LineStyle="Continuous" ss:Weight="1"
             ss:Color="#BFBFBF"/>
          </Borders>
          <Font ss:FontName="Arimo" x:CharSet="204" x:Family="Swiss" ss:Size="11"/>
          <Interior ss:Color="#FFFFFF" ss:Pattern="Solid"/>
          <NumberFormat ss:Format="#,##0"/>
        </Style>
        <Style ss:ID="s39301">
          <Alignment ss:Horizontal="Left" ss:Vertical="Bottom" ss:Indent="2"/>
          <Borders>
            <Border ss:Position="Bottom" ss:LineStyle="Continuous" ss:Weight="1"
             ss:Color="#BFBFBF"/>
            <Border ss:Position="Left" ss:LineStyle="Continuous" ss:Weight="1"
             ss:Color="#808080"/>
            <Border ss:Position="Right" ss:LineStyle="Continuous" ss:Weight="1"
             ss:Color="#808080"/>
            <Border ss:Position="Top" ss:LineStyle="Continuous" ss:Weight="1"
             ss:Color="#BFBFBF"/>
          </Borders>
          <Font ss:FontName="Arimo" x:CharSet="204" x:Family="Swiss" ss:Size="11"
           ss:Bold="1"/>
          <Interior ss:Color="#FFFFFF" ss:Pattern="Solid"/>
          <NumberFormat ss:Format="Standard"/>
        </Style>
        <Style ss:ID="s39302">
          <Alignment ss:Horizontal="Left" ss:Vertical="Bottom" ss:WrapText="1"/>
          <Borders>
            <Border ss:Position="Bottom" ss:LineStyle="Continuous" ss:Weight="1"
             ss:Color="#BFBFBF"/>
            <Border ss:Position="Left" ss:LineStyle="Continuous" ss:Weight="1"
             ss:Color="#808080"/>
            <Border ss:Position="Right" ss:LineStyle="Continuous" ss:Weight="1"
             ss:Color="#808080"/>
            <Border ss:Position="Top" ss:LineStyle="Continuous" ss:Weight="1"
             ss:Color="#BFBFBF"/>
          </Borders>
          <Font ss:FontName="Arimo" x:CharSet="204" x:Family="Swiss" ss:Size="11"
           ss:Bold="1"/>
          <Interior ss:Color="#FFFFFF" ss:Pattern="Solid"/>
          <NumberFormat ss:Format="@"/>
        </Style>
        <Style ss:ID="s39303">
          <Alignment ss:Horizontal="Left" ss:Vertical="Bottom" ss:Indent="1"
           ss:WrapText="1"/>
          <Borders>
            <Border ss:Position="Bottom" ss:LineStyle="Continuous" ss:Weight="1"
             ss:Color="#BFBFBF"/>
            <Border ss:Position="Left" ss:LineStyle="Continuous" ss:Weight="1"
             ss:Color="#808080"/>
            <Border ss:Position="Right" ss:LineStyle="Continuous" ss:Weight="1"
             ss:Color="#808080"/>
            <Border ss:Position="Top" ss:LineStyle="Continuous" ss:Weight="1"
             ss:Color="#BFBFBF"/>
          </Borders>
          <Font ss:FontName="Arimo" x:CharSet="204" x:Family="Swiss" ss:Size="11"/>
          <Interior ss:Color="#FFFFFF" ss:Pattern="Solid"/>
          <NumberFormat ss:Format="@"/>
        </Style>
        <Style ss:ID="s39304">
          <Alignment ss:Horizontal="Left" ss:Vertical="Bottom" ss:WrapText="1"/>
          <Borders>
            <Border ss:Position="Bottom" ss:LineStyle="Continuous" ss:Weight="1"
             ss:Color="#BFBFBF"/>
            <Border ss:Position="Left" ss:LineStyle="Continuous" ss:Weight="1"
             ss:Color="#808080"/>
            <Border ss:Position="Right" ss:LineStyle="Continuous" ss:Weight="1"
             ss:Color="#808080"/>
            <Border ss:Position="Top" ss:LineStyle="Continuous" ss:Weight="1"
             ss:Color="#BFBFBF"/>
          </Borders>
          <Font ss:FontName="Arimo" x:CharSet="204" x:Family="Swiss" ss:Size="11"/>
          <Interior ss:Color="#FFFFFF" ss:Pattern="Solid"/>
          <NumberFormat ss:Format="@"/>
        </Style>
        <Style ss:ID="s39305">
          <Alignment ss:Horizontal="Left" ss:Vertical="Bottom" ss:WrapText="1"/>
          <Borders>
            <Border ss:Position="Bottom" ss:LineStyle="Continuous" ss:Weight="1"
             ss:Color="#BFBFBF"/>
            <Border ss:Position="Left" ss:LineStyle="Continuous" ss:Weight="1"
             ss:Color="#808080"/>
            <Border ss:Position="Right" ss:LineStyle="Continuous" ss:Weight="1"
             ss:Color="#808080"/>
            <Border ss:Position="Top" ss:LineStyle="Continuous" ss:Weight="1"
             ss:Color="#BFBFBF"/>
          </Borders>
          <Font ss:FontName="Arimo" x:CharSet="204" x:Family="Swiss" ss:Size="11"
           ss:Bold="1"/>
          <Interior ss:Color="#FFFFFF" ss:Pattern="Solid"/>
          <NumberFormat/>
        </Style>
        <Style ss:ID="s39307">
          <Alignment ss:Horizontal="Left" ss:Vertical="Bottom"/>
          <Borders>
            <Border ss:Position="Bottom" ss:LineStyle="Continuous" ss:Weight="1"
             ss:Color="#BFBFBF"/>
            <Border ss:Position="Left" ss:LineStyle="Continuous" ss:Weight="1"
             ss:Color="#808080"/>
            <Border ss:Position="Right" ss:LineStyle="Continuous" ss:Weight="1"
             ss:Color="#808080"/>
            <Border ss:Position="Top" ss:LineStyle="Continuous" ss:Weight="1"
             ss:Color="#BFBFBF"/>
          </Borders>
          <Font ss:FontName="Arimo" x:CharSet="204" x:Family="Swiss" ss:Size="11"
           ss:Bold="1"/>
          <Interior ss:Color="#FFFFFF" ss:Pattern="Solid"/>
          <NumberFormat ss:Format="@"/>
        </Style>
        <Style ss:ID="s39308">
          <Alignment ss:Horizontal="Left" ss:Vertical="Bottom"/>
          <Borders>
            <Border ss:Position="Bottom" ss:LineStyle="Continuous" ss:Weight="1"
             ss:Color="#BFBFBF"/>
            <Border ss:Position="Left" ss:LineStyle="Continuous" ss:Weight="1"
             ss:Color="#808080"/>
            <Border ss:Position="Right" ss:LineStyle="Continuous" ss:Weight="1"
             ss:Color="#808080"/>
            <Border ss:Position="Top" ss:LineStyle="Continuous" ss:Weight="1"
             ss:Color="#BFBFBF"/>
          </Borders>
          <Font ss:FontName="Arimo" x:CharSet="204" x:Family="Swiss" ss:Size="11"/>
          <Interior ss:Color="#FFFFFF" ss:Pattern="Solid"/>
          <NumberFormat ss:Format="@"/>
        </Style>
        <Style ss:ID="s39309">
          <Alignment ss:Horizontal="Left" ss:Vertical="Bottom" ss:Indent="1"/>
          <Borders>
            <Border ss:Position="Bottom" ss:LineStyle="Continuous" ss:Weight="1"
             ss:Color="#BFBFBF"/>
            <Border ss:Position="Left" ss:LineStyle="Continuous" ss:Weight="1"
             ss:Color="#808080"/>
            <Border ss:Position="Right" ss:LineStyle="Continuous" ss:Weight="1"
             ss:Color="#808080"/>
            <Border ss:Position="Top" ss:LineStyle="Continuous" ss:Weight="1"
             ss:Color="#BFBFBF"/>
          </Borders>
          <Font ss:FontName="Arimo" x:CharSet="204" x:Family="Swiss" ss:Size="11"/>
          <Interior ss:Color="#FFFFFF" ss:Pattern="Solid"/>
          <NumberFormat ss:Format="@"/>
        </Style>
        <Style ss:ID="s39310">
          <Alignment ss:Horizontal="Left" ss:Vertical="Bottom" ss:Indent="1"/>
          <Borders>
            <Border ss:Position="Bottom" ss:LineStyle="Continuous" ss:Weight="1"
             ss:Color="#BFBFBF"/>
            <Border ss:Position="Left" ss:LineStyle="Continuous" ss:Weight="1"
             ss:Color="#808080"/>
            <Border ss:Position="Right" ss:LineStyle="Continuous" ss:Weight="1"
             ss:Color="#808080"/>
            <Border ss:Position="Top" ss:LineStyle="Continuous" ss:Weight="1"
             ss:Color="#BFBFBF"/>
          </Borders>
          <Font ss:FontName="Arimo" x:CharSet="204" x:Family="Swiss" ss:Size="11"/>
          <Interior ss:Color="#FFFFFF" ss:Pattern="Solid"/>
        </Style>
        <Style ss:ID="s39311">
          <Alignment ss:Horizontal="Left" ss:Vertical="Bottom" ss:Indent="1"/>
          <Borders>
            <Border ss:Position="Bottom" ss:LineStyle="Continuous" ss:Weight="1"
             ss:Color="#BFBFBF"/>
            <Border ss:Position="Left" ss:LineStyle="Continuous" ss:Weight="1"
             ss:Color="#808080"/>
            <Border ss:Position="Right" ss:LineStyle="Continuous" ss:Weight="1"
             ss:Color="#808080"/>
            <Border ss:Position="Top" ss:LineStyle="Continuous" ss:Weight="1"
             ss:Color="#BFBFBF"/>
          </Borders>
          <Font ss:FontName="Arimo" x:CharSet="204" x:Family="Swiss" ss:Size="11"
           ss:Bold="1"/>
          <Interior ss:Color="#FFFFFF" ss:Pattern="Solid"/>
          <NumberFormat ss:Format="Standard"/>
        </Style>
        <Style ss:ID="s39312">
          <Alignment ss:Horizontal="Left" ss:Vertical="Bottom" ss:Indent="1"/>
          <Borders>
            <Border ss:Position="Bottom" ss:LineStyle="Continuous" ss:Weight="1"
             ss:Color="#BFBFBF"/>
            <Border ss:Position="Left" ss:LineStyle="Continuous" ss:Weight="1"
             ss:Color="#808080"/>
            <Border ss:Position="Right" ss:LineStyle="Continuous" ss:Weight="1"
             ss:Color="#808080"/>
            <Border ss:Position="Top" ss:LineStyle="Continuous" ss:Weight="1"
             ss:Color="#BFBFBF"/>
          </Borders>
          <Font ss:FontName="Arimo" x:CharSet="204" x:Family="Swiss" ss:Size="11"
           ss:Bold="1"/>
          <Interior ss:Color="#FFFFFF" ss:Pattern="Solid"/>
        </Style>
        <Style ss:ID="s39313">
          <Alignment ss:Horizontal="Left" ss:Vertical="Bottom" ss:Indent="2"/>
          <Borders>
            <Border ss:Position="Bottom" ss:LineStyle="Continuous" ss:Weight="1"
             ss:Color="#BFBFBF"/>
            <Border ss:Position="Left" ss:LineStyle="Continuous" ss:Weight="1"
             ss:Color="#808080"/>
            <Border ss:Position="Right" ss:LineStyle="Continuous" ss:Weight="1"
             ss:Color="#808080"/>
            <Border ss:Position="Top" ss:LineStyle="Continuous" ss:Weight="1"
             ss:Color="#BFBFBF"/>
          </Borders>
          <Font ss:FontName="Arimo" x:CharSet="204" x:Family="Swiss" ss:Size="11"
           ss:Bold="1"/>
          <Interior ss:Color="#FFFFFF" ss:Pattern="Solid"/>
          <NumberFormat ss:Format="@"/>
        </Style>
        <Style ss:ID="s39314">
          <Alignment ss:Horizontal="Center" ss:Vertical="Bottom"/>
          <Borders>
            <Border ss:Position="Bottom" ss:LineStyle="Continuous" ss:Weight="1"
             ss:Color="#BFBFBF"/>
            <Border ss:Position="Left" ss:LineStyle="Continuous" ss:Weight="1"
             ss:Color="#808080"/>
            <Border ss:Position="Right" ss:LineStyle="Continuous" ss:Weight="1"
             ss:Color="#808080"/>
            <Border ss:Position="Top" ss:LineStyle="Continuous" ss:Weight="1"
             ss:Color="#BFBFBF"/>
          </Borders>
          <Font ss:FontName="Arimo" x:CharSet="204" x:Family="Swiss" ss:Size="11"
           ss:Bold="1"/>
          <Interior ss:Color="#FFFFFF" ss:Pattern="Solid"/>
          <NumberFormat ss:Format="@"/>
        </Style>
        <Style ss:ID="s39315">
          <Alignment ss:Horizontal="Left" ss:Vertical="Bottom" ss:Indent="3"/>
          <Borders>
            <Border ss:Position="Bottom" ss:LineStyle="Continuous" ss:Weight="1"
             ss:Color="#BFBFBF"/>
            <Border ss:Position="Left" ss:LineStyle="Continuous" ss:Weight="1"
             ss:Color="#808080"/>
            <Border ss:Position="Right" ss:LineStyle="Continuous" ss:Weight="1"
             ss:Color="#808080"/>
            <Border ss:Position="Top" ss:LineStyle="Continuous" ss:Weight="1"
             ss:Color="#BFBFBF"/>
          </Borders>
          <Font ss:FontName="Arimo" x:CharSet="204" x:Family="Swiss" ss:Size="11"/>
          <Interior ss:Color="#FFFFFF" ss:Pattern="Solid"/>
        </Style>
        <Style ss:ID="s39316">
          <Alignment ss:Horizontal="Left" ss:Vertical="Bottom" ss:Indent="3"/>
          <Borders>
            <Border ss:Position="Bottom" ss:LineStyle="Continuous" ss:Weight="1"
             ss:Color="#BFBFBF"/>
            <Border ss:Position="Left" ss:LineStyle="Continuous" ss:Weight="1"
             ss:Color="#808080"/>
            <Border ss:Position="Right" ss:LineStyle="Continuous" ss:Weight="1"
             ss:Color="#808080"/>
            <Border ss:Position="Top" ss:LineStyle="Continuous" ss:Weight="1"
             ss:Color="#BFBFBF"/>
          </Borders>
          <Font ss:FontName="Arimo" x:CharSet="204" x:Family="Swiss" ss:Size="11"/>
          <Interior ss:Color="#FFFFFF" ss:Pattern="Solid"/>
          <NumberFormat ss:Format="@"/>
        </Style>
        <Style ss:ID="s39317">
          <Alignment ss:Horizontal="Center" ss:Vertical="Bottom"/>
          <Borders>
            <Border ss:Position="Bottom" ss:LineStyle="Continuous" ss:Weight="1"
             ss:Color="#BFBFBF"/>
            <Border ss:Position="Left" ss:LineStyle="Continuous" ss:Weight="1"
             ss:Color="#808080"/>
            <Border ss:Position="Right" ss:LineStyle="Continuous" ss:Weight="1"
             ss:Color="#808080"/>
            <Border ss:Position="Top" ss:LineStyle="Continuous" ss:Weight="1"
             ss:Color="#BFBFBF"/>
          </Borders>
          <Font ss:FontName="Arimo" x:CharSet="204" x:Family="Swiss" ss:Size="11"/>
          <Interior ss:Color="#FFFFFF" ss:Pattern="Solid"/>
          <NumberFormat ss:Format="@"/>
        </Style>
        <Style ss:ID="s39318">
          <Alignment ss:Horizontal="Left" ss:Vertical="Bottom" ss:Indent="1"/>
          <Borders>
            <Border ss:Position="Bottom" ss:LineStyle="Continuous" ss:Weight="1"
             ss:Color="#BFBFBF"/>
            <Border ss:Position="Left" ss:LineStyle="Continuous" ss:Weight="1"
             ss:Color="#808080"/>
            <Border ss:Position="Right" ss:LineStyle="Continuous" ss:Weight="1"
             ss:Color="#808080"/>
            <Border ss:Position="Top" ss:LineStyle="Continuous" ss:Weight="1"
             ss:Color="#BFBFBF"/>
          </Borders>
          <Font ss:FontName="Arimo" x:CharSet="204" x:Family="Swiss" ss:Size="11"
           ss:Bold="1"/>
          <Interior ss:Color="#FFFFFF" ss:Pattern="Solid"/>
          <NumberFormat ss:Format="@"/>
        </Style>
        <Style ss:ID="s39909">
          <Alignment ss:Horizontal="Left" ss:Vertical="Bottom"/>
          <Borders>
            <Border ss:Position="Bottom" ss:LineStyle="Continuous" ss:Weight="1"
             ss:Color="#BFBFBF"/>
            <Border ss:Position="Left" ss:LineStyle="Continuous" ss:Weight="1"
             ss:Color="#808080"/>
            <Border ss:Position="Right" ss:LineStyle="Continuous" ss:Weight="1"
             ss:Color="#808080"/>
            <Border ss:Position="Top" ss:LineStyle="Continuous" ss:Weight="1"
             ss:Color="#BFBFBF"/>
          </Borders>
          <Font ss:FontName="Arimo" x:CharSet="204" x:Family="Swiss" ss:Size="11"
           ss:Bold="1"/>
          <Interior ss:Color="#FFFFFF" ss:Pattern="Solid"/>
        </Style>
        <Style ss:ID="s39910">
          <Alignment ss:Horizontal="Center" ss:Vertical="Bottom"/>
          <Borders>
            <Border ss:Position="Bottom" ss:LineStyle="Continuous" ss:Weight="1"
             ss:Color="#BFBFBF"/>
            <Border ss:Position="Left" ss:LineStyle="Continuous" ss:Weight="1"
             ss:Color="#808080"/>
            <Border ss:Position="Right" ss:LineStyle="Continuous" ss:Weight="1"
             ss:Color="#808080"/>
            <Border ss:Position="Top" ss:LineStyle="Continuous" ss:Weight="1"
             ss:Color="#BFBFBF"/>
          </Borders>
          <Font ss:FontName="Arimo" x:CharSet="204" x:Family="Swiss" ss:Size="11"
           ss:Bold="1"/>
          <Interior ss:Color="#FFFFFF" ss:Pattern="Solid"/>
        </Style>
        <Style ss:ID="s39911">
          <Alignment ss:Horizontal="Left" ss:Vertical="Bottom"/>
          <Borders>
            <Border ss:Position="Bottom" ss:LineStyle="Continuous" ss:Weight="1"
             ss:Color="#BFBFBF"/>
            <Border ss:Position="Left" ss:LineStyle="Continuous" ss:Weight="1"
             ss:Color="#808080"/>
            <Border ss:Position="Right" ss:LineStyle="Continuous" ss:Weight="1"
             ss:Color="#808080"/>
            <Border ss:Position="Top" ss:LineStyle="Continuous" ss:Weight="1"
             ss:Color="#BFBFBF"/>
          </Borders>
          <Font ss:FontName="Arimo" x:CharSet="204" x:Family="Swiss" ss:Size="11"/>
          <Interior ss:Color="#FFFFFF" ss:Pattern="Solid"/>
        </Style>
        <Style ss:ID="s39912">
          <Alignment ss:Horizontal="Center" ss:Vertical="Bottom"/>
          <Borders>
            <Border ss:Position="Bottom" ss:LineStyle="Continuous" ss:Weight="1"
             ss:Color="#BFBFBF"/>
            <Border ss:Position="Left" ss:LineStyle="Continuous" ss:Weight="1"
             ss:Color="#808080"/>
            <Border ss:Position="Right" ss:LineStyle="Continuous" ss:Weight="1"
             ss:Color="#808080"/>
            <Border ss:Position="Top" ss:LineStyle="Continuous" ss:Weight="1"
             ss:Color="#BFBFBF"/>
          </Borders>
          <Font ss:FontName="Arimo" x:CharSet="204" x:Family="Swiss" ss:Size="11"/>
          <Interior ss:Color="#FFFFFF" ss:Pattern="Solid"/>
        </Style>
        <Style ss:ID="s39914">
          <Alignment ss:Horizontal="Left" ss:Vertical="Bottom"/>
          <Borders>
            <Border ss:Position="Bottom" ss:LineStyle="Continuous" ss:Weight="1"
             ss:Color="#BFBFBF"/>
            <Border ss:Position="Left" ss:LineStyle="Continuous" ss:Weight="1"
             ss:Color="#808080"/>
            <Border ss:Position="Right" ss:LineStyle="Continuous" ss:Weight="1"
             ss:Color="#808080"/>
            <Border ss:Position="Top" ss:LineStyle="Continuous" ss:Weight="1"
             ss:Color="#BFBFBF"/>
          </Borders>
          <Font ss:FontName="Arimo" x:CharSet="204" x:Family="Swiss" ss:Size="11"
           ss:Bold="1"/>
          <Interior ss:Color="#FFFFFF" ss:Pattern="Solid"/>
          <NumberFormat ss:Format="Standard"/>
        </Style>
        <Style ss:ID="s39915">
          <Alignment ss:Horizontal="Center" ss:Vertical="Bottom"/>
          <Borders>
            <Border ss:Position="Bottom" ss:LineStyle="Continuous" ss:Weight="1"
             ss:Color="#BFBFBF"/>
            <Border ss:Position="Left" ss:LineStyle="Continuous" ss:Weight="1"
             ss:Color="#808080"/>
            <Border ss:Position="Right" ss:LineStyle="Continuous" ss:Weight="1"
             ss:Color="#808080"/>
            <Border ss:Position="Top" ss:LineStyle="Continuous" ss:Weight="1"
             ss:Color="#BFBFBF"/>
          </Borders>
          <Font ss:FontName="Arimo" x:CharSet="204" x:Family="Swiss" ss:Size="11"
           ss:Bold="1"/>
          <Interior ss:Color="#FFFFFF" ss:Pattern="Solid"/>
          <NumberFormat ss:Format="#,##0"/>
        </Style>
        <Style ss:ID="s39922">
          <Alignment ss:Horizontal="Left" ss:Vertical="Center"/>
          <Borders/>
          <Font ss:FontName="Arimo" x:CharSet="204" x:Family="Swiss" ss:Size="11"
           ss:Color="#000000" ss:Bold="1"/>
        </Style>
        <Style ss:ID="s39928">
          <Alignment ss:Horizontal="Left" ss:Vertical="Bottom"/>
          <Font ss:FontName="Arimo" x:CharSet="204" x:Family="Swiss" ss:Size="11"/>
          <Interior ss:Color="#FFFFFF" ss:Pattern="Solid"/>
          <Protection/>
        </Style>
        <Style ss:ID="s39934">
          <Alignment ss:Horizontal="Left" ss:Vertical="Center"/>
          <Borders>
            <Border ss:Position="Bottom" ss:LineStyle="Continuous" ss:Weight="1"
             ss:Color="#BFBFBF"/>
            <Border ss:Position="Left" ss:LineStyle="Continuous" ss:Weight="1"
             ss:Color="#808080"/>
            <Border ss:Position="Right" ss:LineStyle="Continuous" ss:Weight="1"
             ss:Color="#808080"/>
            <Border ss:Position="Top" ss:LineStyle="Continuous" ss:Weight="1"
             ss:Color="#808080"/>
          </Borders>
          <Font ss:FontName="Arimo" x:CharSet="204" x:Family="Swiss" ss:Size="11"
           ss:Bold="1"/>
          <Interior ss:Color="#FFCC99" ss:Pattern="Solid"/>
        </Style>
        <Style ss:ID="s39935">
          <Alignment ss:Horizontal="Left" ss:Vertical="Bottom"/>
          <Borders>
            <Border ss:Position="Bottom" ss:LineStyle="Continuous" ss:Weight="1"
             ss:Color="#BFBFBF"/>
            <Border ss:Position="Left" ss:LineStyle="Continuous" ss:Weight="1"
             ss:Color="#808080"/>
            <Border ss:Position="Right" ss:LineStyle="Continuous" ss:Weight="1"
             ss:Color="#808080"/>
            <Border ss:Position="Top" ss:LineStyle="Continuous" ss:Weight="1"
             ss:Color="#808080"/>
          </Borders>
          <Font ss:FontName="Arimo" x:CharSet="204" x:Family="Swiss" ss:Size="11"
           ss:Bold="1"/>
          <Interior ss:Color="#FFCC99" ss:Pattern="Solid"/>
        </Style>
        <Style ss:ID="s40077">
          <Alignment ss:Horizontal="Center" ss:Vertical="Bottom"/>
          <Font ss:FontName="Arimo" x:CharSet="204" x:Family="Swiss" ss:Size="11"/>
          <Interior ss:Color="#FFFFFF" ss:Pattern="Solid"/>
          <Protection/>
        </Style>
        <Style ss:ID="s40078">
          <Alignment ss:Horizontal="Center" ss:Vertical="Bottom"/>
          <Borders>
            <Border ss:Position="Bottom" ss:LineStyle="Continuous" ss:Weight="1"
             ss:Color="#BFBFBF"/>
            <Border ss:Position="Left" ss:LineStyle="Continuous" ss:Weight="1"
             ss:Color="#808080"/>
            <Border ss:Position="Right" ss:LineStyle="Continuous" ss:Weight="1"
             ss:Color="#808080"/>
            <Border ss:Position="Top" ss:LineStyle="Continuous" ss:Weight="1"
             ss:Color="#808080"/>
          </Borders>
          <Font ss:FontName="Arimo" x:CharSet="204" x:Family="Swiss" ss:Size="11"
           ss:Bold="1"/>
          <Interior ss:Color="#FFCC99" ss:Pattern="Solid"/>
        </Style>
        <Style ss:ID="s111">
          <Alignment ss:Horizontal="Right" ss:Vertical="Bottom"/>
          <Borders>
            <Border ss:Position="Bottom" ss:LineStyle="Continuous" ss:Weight="1"
            ss:Color="#BFBFBF"/>
            <Border ss:Position="Left" ss:LineStyle="Continuous" ss:Weight="1"
            ss:Color="#808080"/>
            <Border ss:Position="Right" ss:LineStyle="Continuous" ss:Weight="1"
            ss:Color="#808080"/>
            <Border ss:Position="Top" ss:LineStyle="Continuous" ss:Weight="1"
            ss:Color="#BFBFBF"/>
          </Borders>
          <Font ss:FontName="Arimo" x:CharSet="204" x:Family="Swiss" ss:Size="11"
          ss:Bold="1"/>
          <Interior ss:Color="#FFFFFF" ss:Pattern="Solid"/>
          <NumberFormat ss:Format="#,##0.000"/>
        </Style>
        <Style ss:ID="s109">
          <Alignment ss:Horizontal="Right" ss:Vertical="Bottom"/>
          <Borders>
            <Border ss:Position="Bottom" ss:LineStyle="Continuous" ss:Weight="1"
            ss:Color="#BFBFBF"/>
            <Border ss:Position="Left" ss:LineStyle="Continuous" ss:Weight="1"
            ss:Color="#808080"/>
            <Border ss:Position="Right" ss:LineStyle="Continuous" ss:Weight="1"
            ss:Color="#808080"/>
            <Border ss:Position="Top" ss:LineStyle="Continuous" ss:Weight="1"
            ss:Color="#BFBFBF"/>
          </Borders>
          <Font ss:FontName="Arimo" x:CharSet="204" x:Family="Swiss" ss:Size="11"/>
          <Interior ss:Color="#FFFFFF" ss:Pattern="Solid"/>
          <NumberFormat ss:Format="#,##0.000"/>
        </Style>
        <Style ss:ID="s108">
          <Alignment ss:Horizontal="Right" ss:Vertical="Bottom"/>
          <Borders>
            <Border ss:Position="Bottom" ss:LineStyle="Continuous" ss:Weight="1"
            ss:Color="#BFBFBF"/>
            <Border ss:Position="Left" ss:LineStyle="Continuous" ss:Weight="1"
            ss:Color="#808080"/>
            <Border ss:Position="Right" ss:LineStyle="Continuous" ss:Weight="1"
            ss:Color="#808080"/>
            <Border ss:Position="Top" ss:LineStyle="Continuous" ss:Weight="1"
            ss:Color="#BFBFBF"/>
          </Borders>
          <Font ss:FontName="Arimo" x:CharSet="204" x:Family="Swiss" ss:Size="11"
          ss:Bold="1"/>
          <Interior ss:Color="#FFFFFF" ss:Pattern="Solid"/>
          <NumberFormat ss:Format="Standard"/>
        </Style>
        <Style ss:ID="s108b">
          <Alignment ss:Horizontal="Right" ss:Vertical="Bottom"/>
          <Borders>
            <Border ss:Position="Bottom" ss:LineStyle="Continuous" ss:Weight="1"
            ss:Color="#808080"/>
            <Border ss:Position="Left" ss:LineStyle="Continuous" ss:Weight="1"
            ss:Color="#808080"/>
            <Border ss:Position="Right" ss:LineStyle="Continuous" ss:Weight="1"
            ss:Color="#808080"/>
            <Border ss:Position="Top" ss:LineStyle="Continuous" ss:Weight="1"
            ss:Color="#BFBFBF"/>
          </Borders>
          <Font ss:FontName="Arimo" x:CharSet="204" x:Family="Swiss" ss:Size="11"
          ss:Bold="1"/>
          <Interior ss:Color="#FFFFFF" ss:Pattern="Solid"/>
          <NumberFormat ss:Format="Standard"/>
        </Style>
        <Style ss:ID="s113">
          <Alignment ss:Horizontal="Right" ss:Vertical="Bottom"/>
          <Borders>
            <Border ss:Position="Bottom" ss:LineStyle="Continuous" ss:Weight="1"
            ss:Color="#BFBFBF"/>
            <Border ss:Position="Left" ss:LineStyle="Continuous" ss:Weight="1"
            ss:Color="#808080"/>
            <Border ss:Position="Right" ss:LineStyle="Continuous" ss:Weight="1"
            ss:Color="#808080"/>
            <Border ss:Position="Top" ss:LineStyle="Continuous" ss:Weight="1"
            ss:Color="#BFBFBF"/>
          </Borders>
          <Font ss:FontName="Arimo" x:CharSet="204" x:Family="Swiss" ss:Size="11"/>
          <Interior ss:Color="#FFFFFF" ss:Pattern="Solid"/>
          <NumberFormat ss:Format="Standard"/>
        </Style>
        <Style ss:ID="s119">
          <Alignment ss:Horizontal="Left" ss:Vertical="Center" ss:WrapText="1"/>
          <Borders>
            <Border ss:Position="Bottom" ss:LineStyle="Continuous" ss:Weight="1"
            ss:Color="#BFBFBF"/>
            <Border ss:Position="Left" ss:LineStyle="Continuous" ss:Weight="1"
            ss:Color="#808080"/>
            <Border ss:Position="Right" ss:LineStyle="Continuous" ss:Weight="1"
            ss:Color="#808080"/>
            <Border ss:Position="Top" ss:LineStyle="Continuous" ss:Weight="1"
            ss:Color="#BFBFBF"/>
          </Borders>
          <Font ss:FontName="Arimo" x:CharSet="204" x:Family="Swiss" ss:Size="11"
          ss:Bold="1"/>
          <Interior/>
        </Style>
        <Style ss:ID="s121">
          <Alignment ss:Horizontal="Left" ss:Vertical="Bottom" ss:WrapText="1"/>
          <Borders>
            <Border ss:Position="Bottom" ss:LineStyle="Continuous" ss:Weight="1"
            ss:Color="#BFBFBF"/>
            <Border ss:Position="Left" ss:LineStyle="Continuous" ss:Weight="1"
            ss:Color="#808080"/>
            <Border ss:Position="Right" ss:LineStyle="Continuous" ss:Weight="1"
            ss:Color="#808080"/>
            <Border ss:Position="Top" ss:LineStyle="Continuous" ss:Weight="1"
            ss:Color="#BFBFBF"/>
          </Borders>
          <Font ss:FontName="Arimo" x:CharSet="204" x:Family="Swiss" ss:Size="11"/>
          <Interior/>
        </Style>
        <!-- <<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<< Стили листа ТХ6 >>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>> -->
        <Style ss:ID="TS6Title">
          <Alignment ss:Horizontal="Center" />
          <Font ss:Bold="1" ss:Color="#000000" ss:FontName="Arial Cyr" x:CharSet="204" />
        </Style>
        <Style ss:ID="TS6Column" ss:Parent="Bordered2">
          <Alignment ss:Horizontal="Center" ss:Vertical="Center" ss:WrapText="1" />
          <Font ss:Color="#000000" ss:FontName="Arial Cyr" x:CharSet="204" />
        </Style>
        <Style ss:ID="TS6Cell1" ss:Parent="Bordered1">
          <Font ss:Bold="1" ss:Color="#000000" ss:FontName="Arial Cyr" x:CharSet="204" />
          <Interior ss:Color="#C0C0C0" ss:Pattern="Solid" />
        </Style>
        <Style ss:ID="TS6Cell2" ss:Parent="Bordered1">
          <Font ss:Color="#000000" ss:FontName="Arial Cyr" x:CharSet="204" />
          <Interior ss:Color="#C0C0C0" ss:Pattern="Solid" />
        </Style>
        <Style ss:ID="TS6Cell3" ss:Parent="Bordered1">
          <Font ss:Bold="1" ss:Color="#003366" ss:FontName="Arial Cyr" x:CharSet="204" />
          <Interior ss:Color="#C0C0C0" ss:Pattern="Solid" />
        </Style>
        <Style ss:ID="TS6Cell4" ss:Parent="Bordered1">
          <Font ss:Bold="1" ss:Color="#333333" ss:FontName="Arial Cyr" x:CharSet="204" />
          <Interior ss:Color="#C0C0C0" ss:Pattern="Solid" />
        </Style>
        <Style ss:ID="TS6Cell5" ss:Parent="Bordered1">
          <Font ss:Color="#0066CC" ss:FontName="Arial Cyr" x:CharSet="204" />
        </Style>
        <Style ss:ID="TS6Cell6" ss:Parent="Bordered1">
          <Font ss:Color="#000000" ss:FontName="Arial Cyr" x:CharSet="204" />
        </Style>
        <!-- <<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<< Стили листа ТХ7 >>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>> -->
        <Style ss:ID="TS7Title">
          <Alignment ss:Horizontal="Center" />
          <Font ss:Bold="1" ss:Color="#000000" ss:FontName="Arial Cyr" ss:Size="12" x:CharSet="204" />
        </Style>
        <Style ss:ID="TS7Column" ss:Parent="Bordered2">
          <Alignment ss:Horizontal="Center" ss:Vertical="Center" ss:WrapText="1" />
          <Font ss:Color="#000000" ss:FontName="Arial Cyr" ss:Size="12" x:CharSet="204" />
        </Style>
        <Style ss:ID="TS7Cell1" ss:Parent="Bordered1">
          <Font ss:Color="#000000" ss:FontName="Arial Cyr" x:CharSet="204" />
        </Style>
        <!-- <<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<< Стили листа ТХ8 >>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>> -->
        <Style ss:ID="TS8Title">
          <Alignment ss:Horizontal="Center" />
          <Font ss:Bold="1" ss:Color="#000000" ss:FontName="Arial Cyr" x:CharSet="204" />
        </Style>
        <Style ss:ID="TS8Column" ss:Parent="Bordered2">
          <Alignment ss:Horizontal="Center" ss:Vertical="Center" ss:WrapText="1" />
          <Font ss:Color="#000000" ss:FontName="Arial Cyr" x:CharSet="204" />
        </Style>
        <Style ss:ID="TS8Total" ss:Parent="Bordered1">
          <Font ss:Bold="1" ss:Color="#000000" ss:FontName="Arial Cyr" x:CharSet="204" />
          <Interior ss:Color="#FFCC99" ss:Pattern="Solid" />
        </Style>
        <Style ss:ID="TS8Cell1" ss:Parent="Bordered1">
          <Font ss:Bold="1" ss:Color="#000000" ss:FontName="Arial Cyr" x:CharSet="204" />
          <Interior ss:Color="#C0C0C0" ss:Pattern="Solid" />
        </Style>
        <Style ss:ID="TS8Cell2" ss:Parent="Bordered1">
          <Font ss:Bold="1" ss:Color="#003366" ss:FontName="Arial Cyr" x:CharSet="204" />
          <Interior ss:Color="#C0C0C0" ss:Pattern="Solid" />
        </Style>
        <Style ss:ID="TS8Cell3" ss:Parent="Bordered1">
          <Font ss:Color="#000000" ss:FontName="Arial Cyr" x:CharSet="204" />
          <Interior ss:Color="#C0C0C0" ss:Pattern="Solid" />
        </Style>
        <Style ss:ID="TS8Cell4" ss:Parent="Bordered1">
          <Font ss:Bold="1" ss:Color="#993300" ss:FontName="Arial Cyr" x:CharSet="204" />
          <Interior ss:Color="#C0C0C0" ss:Pattern="Solid" />
        </Style>
        <Style ss:ID="TS8Cell5" ss:Parent="TS8Cell1">
          <Alignment ss:Horizontal="Center" />
        </Style>
        <Style ss:ID="TS8Cell6" ss:Parent="Bordered1">
          <Font ss:Color="#000000" ss:FontName="Arial Cyr" x:CharSet="204" />
        </Style>
        <Style ss:ID="TS8Cell7" ss:Parent="TS8Cell6">
          <Alignment ss:Horizontal="Center" />
        </Style>
        <!-- <<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<< Стили листа ТХ9 >>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>> -->
        <Style ss:ID="TS9Title">
          <Alignment ss:Horizontal="Center" />
          <Font ss:Bold="1" ss:Color="#000000" ss:FontName="Arial Cyr" x:CharSet="204" />
        </Style>
        <Style ss:ID="TS9Column" ss:Parent="Bordered2">
          <Alignment ss:Horizontal="Center" ss:Vertical="Center" ss:WrapText="1" />
          <Font ss:Color="#000000" ss:FontName="Arial Cyr" x:CharSet="204" />
        </Style>
        <Style ss:ID="TS9Total" ss:Parent="Bordered1">
          <Alignment ss:Horizontal="Center" />
          <Font ss:Bold="1" ss:Color="#000000" ss:FontName="Arial Cyr" x:CharSet="204" />
          <Interior ss:Color="#FFCC99" ss:Pattern="Solid" />
        </Style>
        <Style ss:ID="TS9Cell1" ss:Parent="Bordered1">
          <Alignment ss:Horizontal="Center" />
          <Font ss:Bold="1" ss:Color="#000000" ss:FontName="Arial Cyr" x:CharSet="204" />
          <Interior ss:Color="#C5D9F1" ss:Pattern="Solid" />
        </Style>
        <Style ss:ID="TS9Cell2" ss:Parent="Bordered1">
          <Alignment ss:Horizontal="Center" />
          <Font ss:Color="#000000" ss:FontName="Arial Cyr" x:CharSet="204" />
        </Style>
        <!-- <<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<< Стили листа ТХ10 >>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>> -->
        <Style ss:ID="TS10Title">
          <Alignment ss:Horizontal="Center" />
          <Font ss:Bold="1" ss:Color="#000000" ss:FontName="Arial Cyr" x:CharSet="204" />
        </Style>
        <Style ss:ID="TS10Column" ss:Parent="Bordered2">
          <Alignment ss:Horizontal="Center" ss:Vertical="Center" ss:WrapText="1" />
          <Font ss:Color="#000000" ss:FontName="Arial Cyr" x:CharSet="204" />
        </Style>
        <Style ss:ID="TS10NumColumn" ss:Parent="Bordered2">
          <Alignment ss:Horizontal="Center" ss:Vertical="Center" ss:WrapText="1" />
          <Font ss:Bold="1" ss:Color="#000000" ss:FontName="Arial Cyr" x:CharSet="204" />
        </Style>
      </Styles>
      <xsl:apply-templates select="//Sheets" />
    </Workbook>
  </xsl:template>
  <!-- <<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<< Листы шаблона >>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>> -->
  
  
  <xsl:template match="Sheets">
    <xsl:choose>
      
      <!-- <<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<< Лист ТХ32 >>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>> -->
      <xsl:when test="TS32 = 1">
        <Worksheet ss:Name="ВС3">
          <Table>
            <Column ss:AutoFitWidth="0" ss:Width="46.5"/>
            <Column ss:AutoFitWidth="0" ss:Width="1000"/>
            <Column ss:AutoFitWidth="0" ss:Width="127.5"/>
            <Column ss:AutoFitWidth="0" ss:Width="124.5"/>
            <Column ss:AutoFitWidth="0" ss:Width="76.5"/>
            <Column ss:AutoFitWidth="0" ss:Width="78"/>
            <Column ss:AutoFitWidth="0" ss:Width="60"/>
            <Column ss:AutoFitWidth="0" ss:Width="48.75" ss:Span="2"/>
            <Column ss:Index="11" ss:AutoFitWidth="1" ss:Width="54"/>
            <Column ss:AutoFitWidth="1" ss:Width="48.75" ss:Span="45"/>
            <Column ss:Index="58" ss:AutoFitWidth="0" ss:Width="76.5"/>
            
            <Row ss:Height="12.75">
              <Cell ss:Index="2" ss:StyleID="s395"><Data ss:Type="String">Потребители системы водоснабжения</Data></Cell>
            </Row>
            <Row ss:Height="12.75">
              <Cell ss:Index="2" ss:StyleID="s402"><ss:Data ss:Type="String" xmlns="http://www.w3.org/TR/REC-html40"><B>по предприятию <U>___</U></B><I><U><Font html:Color="#FF0000">(указать наименование организации)</Font></U></I><B><I><U>_</U></I><U>_</U></B></ss:Data></Cell>
            </Row>
            <Row ss:Height="12.75">
              <Cell ss:Index="2" ss:StyleID="s402"><ss:Data ss:Type="String" xmlns="http://www.w3.org/TR/REC-html40"><B>на _</B><I><U><Font html:Color="#FF0000">(указать регулируемый период)</Font></U></I><B>_ год</B></ss:Data></Cell>
            </Row>
            <Row ss:Height="12.75"/>
			
            <Row ss:Height="12.75">
              <Cell ss:MergeDown="5" ss:StyleID="s459"><Data ss:Type="String">Разделы</Data></Cell>
              <Cell ss:MergeDown="5" ss:StyleID="s490"><Data ss:Type="String">Категория потребителей</Data></Cell>
              <Cell ss:MergeDown="5" ss:StyleID="m201520164"><Data ss:Type="String">Район</Data></Cell>
              <Cell ss:MergeDown="5" ss:StyleID="m201520184"><Data ss:Type="String">Муниципальное образование</Data></Cell>
              <Cell ss:MergeDown="5" ss:StyleID="s459"><Data ss:Type="String">Наименование населенного пункта</Data></Cell>
              <Cell ss:MergeDown="5" ss:StyleID="s459"><Data ss:Type="String">Наименование котельной</Data></Cell>
              <Cell ss:MergeDown="5" ss:StyleID="s490"><Data ss:Type="String">Идентиф. код  котельной</Data></Cell>
              <Cell ss:MergeAcross="1" ss:MergeDown="3" ss:StyleID="s459"><Data ss:Type="String">Вновь вводимые объекты</Data></Cell>
              <Cell ss:MergeDown="5" ss:StyleID="s479"><Data ss:Type="String">Площадь общего пользования (ОДН), м2</Data></Cell>
              <Cell ss:MergeAcross="7" ss:MergeDown="1" ss:StyleID="s480"><Data ss:Type="String">Нагрузка (мощность) по договорам водоснабжения, м3/час</Data></Cell>
              <Cell ss:MergeAcross="9" ss:MergeDown="1" ss:StyleID="s467"><ss:Data ss:Type="String" xmlns="http://www.w3.org/TR/REC-html40">Утвержденный в тарифе объем в отчетном периоде</ss:Data></Cell>
              <Cell ss:MergeAcross="9" ss:MergeDown="1" ss:StyleID="s467"><Data ss:Type="String">Плановый объем потребления на регулируемый период</Data></Cell>
              <Cell ss:MergeAcross="1" ss:MergeDown="3" ss:StyleID="s467"><Data ss:Type="String">Отклонение плана от тарифа, %</Data></Cell>
              <Cell ss:MergeDown="5" ss:StyleID="s467"><Data ss:Type="String">Фактическое потребление, по приборам учета, м3</Data></Cell>
              <Cell ss:MergeDown="5" ss:StyleID="s467"><Data ss:Type="String">Наличие приборов учета, шт</Data></Cell>
              <Cell ss:MergeDown="5" ss:StyleID="s467"><Data ss:Type="String">Процент установки приборов учета, %</Data></Cell>
              <Cell ss:MergeDown="5" ss:StyleID="s467"><Data ss:Type="String">Причины изменения</Data></Cell>
            
            </Row>
            <Row ss:Height="12.75"/>
            <Row ss:Height="12.75">
				<Cell ss:Index="11" ss:MergeDown="3" ss:StyleID="s467"><Data ss:Type="String">Всего, м3/час</Data></Cell>
				<Cell ss:MergeAcross="6" ss:StyleID="s467"><Data ss:Type="String">в том числе</Data></Cell>

				<Cell ss:Index="19" ss:MergeDown="3" ss:StyleID="s467"><Data ss:Type="String">Степень благоустройства</Data></Cell>
				<Cell ss:MergeDown="3" ss:StyleID="s467"><Data ss:Type="String">Количество потребителей, чел</Data></Cell>
				<Cell ss:MergeDown="3" ss:StyleID="s467"><Data ss:Type="String">Всего, м3/час</Data></Cell>

				<Cell ss:Index="22" ss:MergeAcross="6" ss:StyleID="s467"><Data ss:Type="String">в том числе</Data></Cell>

				<Cell ss:Index="29" ss:MergeDown="3" ss:StyleID="s467"><Data ss:Type="String">Степень благоустройства</Data></Cell>
				<Cell ss:MergeDown="3" ss:StyleID="s467"><Data ss:Type="String">Количество потребителей, чел</Data></Cell>
				<Cell ss:MergeDown="3" ss:StyleID="s467"><Data ss:Type="String">Всего, м3/час</Data></Cell>

				<Cell ss:Index="32" ss:MergeAcross="6" ss:StyleID="s467"><Data ss:Type="String">в том числе</Data></Cell>

			</Row>
			<Row ss:Height="12.75">
				<Cell ss:Index="12" ss:MergeAcross="2" ss:StyleID="s480"><Data ss:Type="String">на холодное водоснабжение</Data></Cell>
				<Cell ss:MergeAcross="3" ss:StyleID="s480"><Data ss:Type="String">на горячее водоснабжение</Data></Cell>

				<Cell ss:Index="22" ss:MergeAcross="2" ss:StyleID="s480"><Data ss:Type="String">на холодное водоснабжение</Data></Cell>
				<Cell ss:MergeAcross="3" ss:StyleID="s480"><Data ss:Type="String">на горячее водоснабжение</Data></Cell>

				<Cell ss:Index="32" ss:MergeAcross="2" ss:StyleID="s480"><Data ss:Type="String">на холодное водоснабжение</Data></Cell>
				<Cell ss:MergeAcross="3" ss:StyleID="s480"><Data ss:Type="String">на горячее водоснабжение</Data></Cell>
			
			</Row>
			<Row ss:Height="12.75">
				
				<Cell ss:Index="8" ss:MergeDown="1" ss:StyleID="s467"><Data ss:Type="String">кол-во, шт</Data></Cell>
				<Cell ss:MergeDown="1" ss:StyleID="s467"><Data ss:Type="String">дата ввода</Data></Cell>

				
				<Cell ss:Index="12" ss:MergeDown="1" ss:StyleID="s467"><Data ss:Type="String">централизованное</Data></Cell>
				<Cell ss:MergeDown="1" ss:StyleID="s467"><Data ss:Type="String">из открытой системы ТС</Data></Cell>
				<Cell ss:MergeDown="1" ss:StyleID="s467"><Data ss:Type="String">на ОДН</Data></Cell>
				<Cell ss:MergeDown="1" ss:StyleID="s467"><Data ss:Type="String">централизованное</Data></Cell>
				<Cell ss:MergeDown="1" ss:StyleID="s467"><Data ss:Type="String">из открытой системы ТС</Data></Cell>
				<Cell ss:MergeDown="1" ss:StyleID="s467"><Data ss:Type="String">теплообменники</Data></Cell>
				<Cell ss:MergeDown="1" ss:StyleID="s467"><Data ss:Type="String">на ОДН</Data></Cell>


				<Cell ss:Index="22" ss:MergeDown="1" ss:StyleID="s467"><Data ss:Type="String">централизованное</Data></Cell>
				<Cell ss:MergeDown="1" ss:StyleID="s467"><Data ss:Type="String">из открытой системы ТС</Data></Cell>
				<Cell ss:MergeDown="1" ss:StyleID="s467"><Data ss:Type="String">на ОДН</Data></Cell>
				<Cell ss:MergeDown="1" ss:StyleID="s467"><Data ss:Type="String">централизованное</Data></Cell>
				<Cell ss:MergeDown="1" ss:StyleID="s467"><Data ss:Type="String">из открытой системы ТС</Data></Cell>
				<Cell ss:MergeDown="1" ss:StyleID="s467"><Data ss:Type="String">теплообменники</Data></Cell>
				<Cell ss:MergeDown="1" ss:StyleID="s467"><Data ss:Type="String">на ОДН</Data></Cell>

				<Cell ss:Index="32" ss:MergeDown="1" ss:StyleID="s467"><Data ss:Type="String">централизованное</Data></Cell>
				<Cell ss:MergeDown="1" ss:StyleID="s467"><Data ss:Type="String">из открытой системы ТС</Data></Cell>
				<Cell ss:MergeDown="1" ss:StyleID="s467"><Data ss:Type="String">на ОДН</Data></Cell>
				<Cell ss:MergeDown="1" ss:StyleID="s467"><Data ss:Type="String">централизованное</Data></Cell>
				<Cell ss:MergeDown="1" ss:StyleID="s467"><Data ss:Type="String">из открытой системы ТС</Data></Cell>
				<Cell ss:MergeDown="1" ss:StyleID="s467"><Data ss:Type="String">теплообменники</Data></Cell>
				<Cell ss:MergeDown="1" ss:StyleID="s467"><Data ss:Type="String">на ОДН</Data></Cell>

				<Cell ss:MergeDown="1" ss:StyleID="s467"><Data ss:Type="String">кол-во потребителей, чел</Data></Cell>
				<Cell ss:MergeDown="1" ss:StyleID="s467"><Data ss:Type="String">потребление воды</Data></Cell>
			</Row>
			<Row ss:Height="12.75">
				
				
			</Row>
			
			
            <Row ss:Height="12.75">
              <Cell ss:StyleID="s412"><Data ss:Type="Number">1</Data></Cell>
              <Cell ss:StyleID="s412"><Data ss:Type="Number">2</Data></Cell>
			  <Cell ss:StyleID="s412"/>
			  <Cell ss:StyleID="s412"/>
              <Cell ss:StyleID="s412"><Data ss:Type="Number">3</Data></Cell>
              <Cell ss:StyleID="s412"><Data ss:Type="Number">4</Data></Cell>
              <Cell ss:StyleID="s412"><Data ss:Type="Number">5</Data></Cell>
              <Cell ss:StyleID="s412"><Data ss:Type="Number">6</Data></Cell>
              <Cell ss:StyleID="s412"><Data ss:Type="Number">7</Data></Cell>
              <Cell ss:StyleID="s412"><Data ss:Type="Number">8</Data></Cell>
              <Cell ss:StyleID="s412"><Data ss:Type="Number">9</Data></Cell>
              <Cell ss:StyleID="s412"><Data ss:Type="Number">10</Data></Cell>
              <Cell ss:StyleID="s412"><Data ss:Type="Number">11</Data></Cell>
              <Cell ss:StyleID="s412"><Data ss:Type="Number">12</Data></Cell>
              <Cell ss:StyleID="s412"><Data ss:Type="Number">13</Data></Cell>
              <Cell ss:StyleID="s412"><Data ss:Type="Number">14</Data></Cell>
              <Cell ss:StyleID="s412"><Data ss:Type="Number">15</Data></Cell>
              <Cell ss:StyleID="s412"><Data ss:Type="Number">16</Data></Cell>
              <Cell ss:StyleID="s412"><Data ss:Type="Number">17</Data></Cell>
              <Cell ss:StyleID="s412"><Data ss:Type="Number">18</Data></Cell>
              <Cell ss:StyleID="s412"><Data ss:Type="Number">19</Data></Cell>
              <Cell ss:StyleID="s412"><Data ss:Type="Number">20</Data></Cell>
              <Cell ss:StyleID="s412"><Data ss:Type="Number">21</Data></Cell>
              <Cell ss:StyleID="s412"><Data ss:Type="Number">22</Data></Cell>
              <Cell ss:StyleID="s412"><Data ss:Type="Number">23</Data></Cell>
              <Cell ss:StyleID="s412"><Data ss:Type="Number">24</Data></Cell>
              <Cell ss:StyleID="s412"><Data ss:Type="Number">25</Data></Cell>
              <Cell ss:StyleID="s412"><Data ss:Type="Number">26</Data></Cell>
              <Cell ss:StyleID="s412"><Data ss:Type="Number">27</Data></Cell>
              <Cell ss:StyleID="s412"><Data ss:Type="Number">28</Data></Cell>
              <Cell ss:StyleID="s412"><Data ss:Type="Number">29</Data></Cell>
              <Cell ss:StyleID="s412"><Data ss:Type="Number">30</Data></Cell>
              <Cell ss:StyleID="s412"><Data ss:Type="Number">31</Data></Cell>
              <Cell ss:StyleID="s412"><Data ss:Type="Number">32</Data></Cell>
              <Cell ss:StyleID="s412"><Data ss:Type="Number">33</Data></Cell>
              <Cell ss:StyleID="s412"><Data ss:Type="Number">34</Data></Cell>
              <Cell ss:StyleID="s412"><Data ss:Type="Number">35</Data></Cell>
              <Cell ss:StyleID="s412"><Data ss:Type="Number">36</Data></Cell>
              <Cell ss:StyleID="s412"><Data ss:Type="Number">37</Data></Cell>
              <Cell ss:StyleID="s412"><Data ss:Type="Number">38</Data></Cell>
              <Cell ss:StyleID="s412"><Data ss:Type="Number">39</Data></Cell>
              <Cell ss:StyleID="s412"><Data ss:Type="Number">40</Data></Cell>
              <Cell ss:StyleID="s412"><Data ss:Type="Number">41</Data></Cell>
              <Cell ss:StyleID="s412"><Data ss:Type="Number">42</Data></Cell>
            </Row>
            <!-- <<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<< Данные таблицы >>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>> -->
            <xsl:apply-templates select="//TS32" />
          </Table>
          <WorksheetOptions xmlns="urn:schemas-microsoft-com:office:excel">
            <Zoom>80</Zoom>
          </WorksheetOptions>
        </Worksheet>
      
      </xsl:when>
      
      <!-- <<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<< Лист ТХ12 >>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>> -->
      <xsl:when test="TS12 = 1">
        <Worksheet ss:Name="ВС1">
          <Table>
            <Column ss:AutoFitWidth="0" ss:Width="46.5"/>
            <Column ss:AutoFitWidth="0" ss:Width="1000"/>
            <Column ss:AutoFitWidth="0" ss:Width="127.5"/>
            <Column ss:AutoFitWidth="0" ss:Width="124.5"/>
            <Column ss:AutoFitWidth="0" ss:Width="76.5"/>
            <Column ss:AutoFitWidth="0" ss:Width="78"/>
            <Column ss:AutoFitWidth="0" ss:Width="60"/>
            <Column ss:AutoFitWidth="0" ss:Width="48.75" ss:Span="2"/>
            <Column ss:Index="11" ss:AutoFitWidth="1" ss:Width="54"/>
            <Column ss:AutoFitWidth="1" ss:Width="48.75" ss:Span="45"/>
            <Column ss:Index="58" ss:AutoFitWidth="0" ss:Width="76.5"/>
            
            <Row ss:Height="12.75">
              <Cell ss:Index="2" ss:StyleID="s395"><Data ss:Type="String">Потребители системы водоснабжения</Data></Cell>
            </Row>
            <Row ss:Height="12.75">
              <Cell ss:Index="2" ss:StyleID="s402"><ss:Data ss:Type="String" xmlns="http://www.w3.org/TR/REC-html40"><B>по предприятию <U>___</U></B><I><U><Font html:Color="#FF0000">(указать наименование организации)</Font></U></I><B><I><U>_</U></I><U>_</U></B></ss:Data></Cell>
            </Row>
            <Row ss:Height="12.75">
              <Cell ss:Index="2" ss:StyleID="s402"><ss:Data ss:Type="String" xmlns="http://www.w3.org/TR/REC-html40"><B>на _</B><I><U><Font html:Color="#FF0000">(указать регулируемый период)</Font></U></I><B>_ год</B></ss:Data></Cell>
            </Row>
            <Row ss:Height="12.75"/>
			
            <Row ss:Height="12.75">
              <Cell ss:MergeDown="5" ss:StyleID="s459"><Data ss:Type="String">Разделы</Data></Cell>
              <Cell ss:MergeDown="5" ss:StyleID="s490"><Data ss:Type="String">Категория потребителей</Data></Cell>
              <Cell ss:MergeDown="5" ss:StyleID="m201520164"><Data ss:Type="String">Район</Data></Cell>
              <Cell ss:MergeDown="5" ss:StyleID="m201520184"><Data ss:Type="String">Муниципальное образование</Data></Cell>
              <Cell ss:MergeDown="5" ss:StyleID="s459"><Data ss:Type="String">Наименование населенного пункта</Data></Cell>
              <Cell ss:MergeDown="5" ss:StyleID="s459"><Data ss:Type="String">Наименование котельной</Data></Cell>
              <Cell ss:MergeDown="5" ss:StyleID="s490"><Data ss:Type="String">Идентиф. код  котельной</Data></Cell>
              <Cell ss:MergeAcross="1" ss:MergeDown="3" ss:StyleID="s459"><Data ss:Type="String">Вновь вводимые объекты</Data></Cell>
              <Cell ss:MergeDown="5" ss:StyleID="s479"><Data ss:Type="String">Площадь общего пользования (ОДН), м2</Data></Cell>
              <Cell ss:MergeAcross="7" ss:MergeDown="1" ss:StyleID="s480"><Data ss:Type="String">Нагрузка (мощность) по договорам водоснабжения, м3/час</Data></Cell>
              <Cell ss:MergeAcross="9" ss:MergeDown="1" ss:StyleID="s467"><ss:Data ss:Type="String" xmlns="http://www.w3.org/TR/REC-html40">Утвержденный в тарифе объем в отчетном периоде</ss:Data></Cell>
              <Cell ss:MergeAcross="9" ss:MergeDown="1" ss:StyleID="s467"><Data ss:Type="String">Плановый объем потребления на регулируемый период</Data></Cell>
              <Cell ss:MergeAcross="1" ss:MergeDown="3" ss:StyleID="s467"><Data ss:Type="String">Отклонение плана от тарифа, %</Data></Cell>
              <Cell ss:MergeDown="5" ss:StyleID="s467"><Data ss:Type="String">Фактическое потребление, по приборам учета, м3</Data></Cell>
              <Cell ss:MergeDown="5" ss:StyleID="s467"><Data ss:Type="String">Наличие приборов учета, шт</Data></Cell>
              <Cell ss:MergeDown="5" ss:StyleID="s467"><Data ss:Type="String">Процент установки приборов учета, %</Data></Cell>
              <Cell ss:MergeDown="5" ss:StyleID="s467"><Data ss:Type="String">Причины изменения</Data></Cell>
            
            </Row>
            <Row ss:Height="12.75"/>
            <Row ss:Height="12.75">
				<Cell ss:Index="11" ss:MergeDown="3" ss:StyleID="s467"><Data ss:Type="String">Всего, м3/час</Data></Cell>
				<Cell ss:MergeAcross="6" ss:StyleID="s467"><Data ss:Type="String">в том числе</Data></Cell>

				<Cell ss:Index="19" ss:MergeDown="3" ss:StyleID="s467"><Data ss:Type="String">Степень благоустройства</Data></Cell>
				<Cell ss:MergeDown="3" ss:StyleID="s467"><Data ss:Type="String">Количество потребителей, чел</Data></Cell>
				<Cell ss:MergeDown="3" ss:StyleID="s467"><Data ss:Type="String">Всего, м3/час</Data></Cell>

				<Cell ss:Index="22" ss:MergeAcross="6" ss:StyleID="s467"><Data ss:Type="String">в том числе</Data></Cell>

				<Cell ss:Index="29" ss:MergeDown="3" ss:StyleID="s467"><Data ss:Type="String">Степень благоустройства</Data></Cell>
				<Cell ss:MergeDown="3" ss:StyleID="s467"><Data ss:Type="String">Количество потребителей, чел</Data></Cell>
				<Cell ss:MergeDown="3" ss:StyleID="s467"><Data ss:Type="String">Всего, м3/час</Data></Cell>

				<Cell ss:Index="32" ss:MergeAcross="6" ss:StyleID="s467"><Data ss:Type="String">в том числе</Data></Cell>

			</Row>
			<Row ss:Height="12.75">
				<Cell ss:Index="12" ss:MergeAcross="2" ss:StyleID="s480"><Data ss:Type="String">на холодное водоснабжение</Data></Cell>
				<Cell ss:MergeAcross="3" ss:StyleID="s480"><Data ss:Type="String">на горячее водоснабжение</Data></Cell>

				<Cell ss:Index="22" ss:MergeAcross="2" ss:StyleID="s480"><Data ss:Type="String">на холодное водоснабжение</Data></Cell>
				<Cell ss:MergeAcross="3" ss:StyleID="s480"><Data ss:Type="String">на горячее водоснабжение</Data></Cell>

				<Cell ss:Index="32" ss:MergeAcross="2" ss:StyleID="s480"><Data ss:Type="String">на холодное водоснабжение</Data></Cell>
				<Cell ss:MergeAcross="3" ss:StyleID="s480"><Data ss:Type="String">на горячее водоснабжение</Data></Cell>
			
			</Row>
			<Row ss:Height="12.75">
				
				<Cell ss:Index="8" ss:MergeDown="1" ss:StyleID="s467"><Data ss:Type="String">кол-во, шт</Data></Cell>
				<Cell ss:MergeDown="1" ss:StyleID="s467"><Data ss:Type="String">дата ввода</Data></Cell>

				
				<Cell ss:Index="12" ss:MergeDown="1" ss:StyleID="s467"><Data ss:Type="String">централизованное</Data></Cell>
				<Cell ss:MergeDown="1" ss:StyleID="s467"><Data ss:Type="String">из открытой системы ТС</Data></Cell>
				<Cell ss:MergeDown="1" ss:StyleID="s467"><Data ss:Type="String">на ОДН</Data></Cell>
				<Cell ss:MergeDown="1" ss:StyleID="s467"><Data ss:Type="String">централизованное</Data></Cell>
				<Cell ss:MergeDown="1" ss:StyleID="s467"><Data ss:Type="String">из открытой системы ТС</Data></Cell>
				<Cell ss:MergeDown="1" ss:StyleID="s467"><Data ss:Type="String">теплообменники</Data></Cell>
				<Cell ss:MergeDown="1" ss:StyleID="s467"><Data ss:Type="String">на ОДН</Data></Cell>


				<Cell ss:Index="22" ss:MergeDown="1" ss:StyleID="s467"><Data ss:Type="String">централизованное</Data></Cell>
				<Cell ss:MergeDown="1" ss:StyleID="s467"><Data ss:Type="String">из открытой системы ТС</Data></Cell>
				<Cell ss:MergeDown="1" ss:StyleID="s467"><Data ss:Type="String">на ОДН</Data></Cell>
				<Cell ss:MergeDown="1" ss:StyleID="s467"><Data ss:Type="String">централизованное</Data></Cell>
				<Cell ss:MergeDown="1" ss:StyleID="s467"><Data ss:Type="String">из открытой системы ТС</Data></Cell>
				<Cell ss:MergeDown="1" ss:StyleID="s467"><Data ss:Type="String">теплообменники</Data></Cell>
				<Cell ss:MergeDown="1" ss:StyleID="s467"><Data ss:Type="String">на ОДН</Data></Cell>

				<Cell ss:Index="32" ss:MergeDown="1" ss:StyleID="s467"><Data ss:Type="String">централизованное</Data></Cell>
				<Cell ss:MergeDown="1" ss:StyleID="s467"><Data ss:Type="String">из открытой системы ТС</Data></Cell>
				<Cell ss:MergeDown="1" ss:StyleID="s467"><Data ss:Type="String">на ОДН</Data></Cell>
				<Cell ss:MergeDown="1" ss:StyleID="s467"><Data ss:Type="String">централизованное</Data></Cell>
				<Cell ss:MergeDown="1" ss:StyleID="s467"><Data ss:Type="String">из открытой системы ТС</Data></Cell>
				<Cell ss:MergeDown="1" ss:StyleID="s467"><Data ss:Type="String">теплообменники</Data></Cell>
				<Cell ss:MergeDown="1" ss:StyleID="s467"><Data ss:Type="String">на ОДН</Data></Cell>

				<Cell ss:MergeDown="1" ss:StyleID="s467"><Data ss:Type="String">кол-во потребителей, чел</Data></Cell>
				<Cell ss:MergeDown="1" ss:StyleID="s467"><Data ss:Type="String">потребление воды</Data></Cell>
			</Row>
			<Row ss:Height="12.75">
				
				
			</Row>
			
			
            <Row ss:Height="12.75">
              <Cell ss:StyleID="s412"><Data ss:Type="Number">1</Data></Cell>
              <Cell ss:StyleID="s412"><Data ss:Type="Number">2</Data></Cell>
			  <Cell ss:StyleID="s412"/>
			  <Cell ss:StyleID="s412"/>
              <Cell ss:StyleID="s412"><Data ss:Type="Number">3</Data></Cell>
              <Cell ss:StyleID="s412"><Data ss:Type="Number">4</Data></Cell>
              <Cell ss:StyleID="s412"><Data ss:Type="Number">5</Data></Cell>
              <Cell ss:StyleID="s412"><Data ss:Type="Number">6</Data></Cell>
              <Cell ss:StyleID="s412"><Data ss:Type="Number">7</Data></Cell>
              <Cell ss:StyleID="s412"><Data ss:Type="Number">8</Data></Cell>
              <Cell ss:StyleID="s412"><Data ss:Type="Number">9</Data></Cell>
              <Cell ss:StyleID="s412"><Data ss:Type="Number">10</Data></Cell>
              <Cell ss:StyleID="s412"><Data ss:Type="Number">11</Data></Cell>
              <Cell ss:StyleID="s412"><Data ss:Type="Number">12</Data></Cell>
              <Cell ss:StyleID="s412"><Data ss:Type="Number">13</Data></Cell>
              <Cell ss:StyleID="s412"><Data ss:Type="Number">14</Data></Cell>
              <Cell ss:StyleID="s412"><Data ss:Type="Number">15</Data></Cell>
              <Cell ss:StyleID="s412"><Data ss:Type="Number">16</Data></Cell>
              <Cell ss:StyleID="s412"><Data ss:Type="Number">17</Data></Cell>
              <Cell ss:StyleID="s412"><Data ss:Type="Number">18</Data></Cell>
              <Cell ss:StyleID="s412"><Data ss:Type="Number">19</Data></Cell>
              <Cell ss:StyleID="s412"><Data ss:Type="Number">20</Data></Cell>
              <Cell ss:StyleID="s412"><Data ss:Type="Number">21</Data></Cell>
              <Cell ss:StyleID="s412"><Data ss:Type="Number">22</Data></Cell>
              <Cell ss:StyleID="s412"><Data ss:Type="Number">23</Data></Cell>
              <Cell ss:StyleID="s412"><Data ss:Type="Number">24</Data></Cell>
              <Cell ss:StyleID="s412"><Data ss:Type="Number">25</Data></Cell>
              <Cell ss:StyleID="s412"><Data ss:Type="Number">26</Data></Cell>
              <Cell ss:StyleID="s412"><Data ss:Type="Number">27</Data></Cell>
              <Cell ss:StyleID="s412"><Data ss:Type="Number">28</Data></Cell>
              <Cell ss:StyleID="s412"><Data ss:Type="Number">29</Data></Cell>
              <Cell ss:StyleID="s412"><Data ss:Type="Number">30</Data></Cell>
              <Cell ss:StyleID="s412"><Data ss:Type="Number">31</Data></Cell>
              <Cell ss:StyleID="s412"><Data ss:Type="Number">32</Data></Cell>
              <Cell ss:StyleID="s412"><Data ss:Type="Number">33</Data></Cell>
              <Cell ss:StyleID="s412"><Data ss:Type="Number">34</Data></Cell>
              <Cell ss:StyleID="s412"><Data ss:Type="Number">35</Data></Cell>
              <Cell ss:StyleID="s412"><Data ss:Type="Number">36</Data></Cell>
              <Cell ss:StyleID="s412"><Data ss:Type="Number">37</Data></Cell>
              <Cell ss:StyleID="s412"><Data ss:Type="Number">38</Data></Cell>
              <Cell ss:StyleID="s412"><Data ss:Type="Number">39</Data></Cell>
              <Cell ss:StyleID="s412"><Data ss:Type="Number">40</Data></Cell>
              <Cell ss:StyleID="s412"><Data ss:Type="Number">41</Data></Cell>
              <Cell ss:StyleID="s412"><Data ss:Type="Number">42</Data></Cell>
            </Row>
            <!-- <<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<< Данные таблицы >>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>> -->
            <xsl:apply-templates select="//TS12" />
          </Table>
          <WorksheetOptions xmlns="urn:schemas-microsoft-com:office:excel">
            <Zoom>80</Zoom>
          </WorksheetOptions>
        </Worksheet>
      
      </xsl:when>
      <!-- <<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<< Лист ТХ11 >>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>> -->
      <xsl:when test="TS11 = 1">
        <Worksheet ss:Name="ВО1">
          <Table>
            <Column ss:AutoFitWidth="0" ss:Width="46.5"/>
            <Column ss:AutoFitWidth="0" ss:Width="1000"/>
            <Column ss:AutoFitWidth="0" ss:Width="127.5"/>
            <Column ss:AutoFitWidth="0" ss:Width="124.5"/>
            <Column ss:AutoFitWidth="0" ss:Width="76.5"/>
            <Column ss:AutoFitWidth="0" ss:Width="78"/>
            <Column ss:AutoFitWidth="0" ss:Width="60"/>
            <Column ss:AutoFitWidth="0" ss:Width="48.75" ss:Span="2"/>
            <Column ss:Index="11" ss:AutoFitWidth="1" ss:Width="54"/>
            <Column ss:AutoFitWidth="1" ss:Width="48.75" ss:Span="45"/>
            <Column ss:Index="58" ss:AutoFitWidth="0" ss:Width="76.5"/>
            
            <Row ss:Height="12.75">
              <Cell ss:Index="2" ss:StyleID="s395"><Data ss:Type="String">Потребители системы водоотведения</Data></Cell>
            </Row>
            <Row ss:Height="12.75">
              <Cell ss:Index="2" ss:StyleID="s402"><ss:Data ss:Type="String" xmlns="http://www.w3.org/TR/REC-html40"><B>по предприятию <U>___</U></B><I><U><Font html:Color="#FF0000">(указать наименование организации)</Font></U></I><B><I><U>_</U></I><U>_</U></B></ss:Data></Cell>
            </Row>
            <Row ss:Height="12.75">
              <Cell ss:Index="2" ss:StyleID="s402"><ss:Data ss:Type="String" xmlns="http://www.w3.org/TR/REC-html40"><B>на _</B><I><U><Font html:Color="#FF0000">(указать регулируемый период)</Font></U></I><B>_ год</B></ss:Data></Cell>
            </Row>
            <Row ss:Height="12.75"/>
			
            <Row ss:Height="12.75">
              <Cell ss:MergeDown="6" ss:StyleID="s459"><Data ss:Type="String">№ п/п</Data></Cell>
              <Cell ss:MergeDown="6" ss:StyleID="s490"><Data ss:Type="String">Категория потребителя</Data></Cell>
              <Cell ss:MergeDown="6" ss:StyleID="m201520164"><Data ss:Type="String">Район</Data></Cell>
              <Cell ss:MergeDown="6" ss:StyleID="m201520184"><Data ss:Type="String">Наслег</Data></Cell>
              <Cell ss:MergeDown="6" ss:StyleID="s459"><Data ss:Type="String">населенного пункта</Data></Cell>
              <Cell ss:MergeDown="6" ss:StyleID="s459"><Data ss:Type="String">Наименование котельной</Data></Cell>
              <Cell ss:MergeDown="6" ss:StyleID="s490"><Data ss:Type="String">Идентиф. код  котельной</Data></Cell>
              <Cell ss:MergeAcross="1" ss:MergeDown="3" ss:StyleID="s459"><Data ss:Type="String">Вновь вводимые объекты</Data></Cell>
              <Cell ss:MergeAcross="8" ss:MergeDown="1" ss:StyleID="s479"><Data ss:Type="String">Нагрузка (мощность) по договорам водоотведения, м3/час</Data></Cell>
              <Cell ss:MergeAcross="10" ss:MergeDown="1" ss:StyleID="s480"><Data ss:Type="String">Утвержденный в тарифе объем в отчетном периоде</Data></Cell>
              <Cell ss:MergeAcross="10" ss:MergeDown="1" ss:StyleID="s467"><ss:Data ss:Type="String" xmlns="http://www.w3.org/TR/REC-html40">Плановый объем потребления на регулируемый период</ss:Data></Cell>
              <Cell ss:MergeAcross="1" ss:MergeDown="3" ss:StyleID="s467"><Data ss:Type="String">Отклонение плана от тарифа, %</Data></Cell>
              <Cell ss:MergeDown="6" ss:StyleID="s467"><Data ss:Type="String">Фактическое потребление, по приборам учета, м3</Data></Cell>
              <Cell ss:MergeDown="6" ss:StyleID="s467"><Data ss:Type="String">Наличие приборов учета, шт</Data></Cell>
              <Cell ss:MergeDown="6" ss:StyleID="s467"><Data ss:Type="String">Процент установки приборов учета, %</Data></Cell>
              <Cell ss:MergeDown="6" ss:StyleID="s467"><Data ss:Type="String">Причины изменения</Data></Cell>
            
            </Row>
            <Row ss:Height="12.75"/>
            <Row ss:Height="12.75">
			 <Cell ss:Index="10" ss:MergeDown="4" ss:StyleID="s467"><Data ss:Type="String">Всего, М3/час</Data></Cell>
			 <Cell ss:MergeAcross="7" ss:StyleID="s467"><Data ss:Type="String">в том числе</Data></Cell>
			 <Cell ss:MergeDown="4" ss:StyleID="s467"><Data ss:Type="String">Степень благоустройства</Data></Cell>
			 <Cell ss:MergeDown="4" ss:StyleID="s467"><Data ss:Type="String">Количество потребителей, чел</Data></Cell>
			 <Cell ss:MergeDown="4" ss:StyleID="s467"><Data ss:Type="String">Всего, м3</Data></Cell>
			 <Cell ss:MergeAcross="7" ss:StyleID="s467"><Data ss:Type="String">в том числе</Data></Cell>
			 <Cell ss:MergeDown="4" ss:StyleID="s467"><Data ss:Type="String">Степень благоустройства</Data></Cell>
			 <Cell ss:MergeDown="4" ss:StyleID="s467"><Data ss:Type="String">Количество потребителей, чел</Data></Cell>
			 <Cell ss:MergeDown="4" ss:StyleID="s467"><Data ss:Type="String">Всего, м3</Data></Cell>
			 <Cell ss:MergeAcross="7" ss:StyleID="s467"><Data ss:Type="String">в том числе</Data></Cell>
			</Row>
			<Row ss:Height="12.75">
				<Cell ss:Index="11" ss:MergeAcross="3" ss:MergeDown="2" ss:StyleID="s467"><Data ss:Type="String">коллекторная канализация</Data></Cell>
				<Cell  ss:MergeAcross="3" ss:MergeDown="2" ss:StyleID="s467"><Data ss:Type="String">вывозная канализация</Data></Cell>

				<Cell ss:Index="22" ss:MergeAcross="3" ss:MergeDown="2" ss:StyleID="s467"><Data ss:Type="String">коллекторная канализация</Data></Cell>
				<Cell  ss:MergeAcross="3" ss:MergeDown="2" ss:StyleID="s467"><Data ss:Type="String">вывозная канализация</Data></Cell>

				<Cell ss:Index="33" ss:MergeAcross="3" ss:MergeDown="2" ss:StyleID="s467"><Data ss:Type="String">коллекторная канализация</Data></Cell>
				<Cell  ss:MergeAcross="3" ss:MergeDown="2" ss:StyleID="s467"><Data ss:Type="String">вывозная канализация</Data></Cell>
			</Row>
			<Row ss:Height="12.75">
				<Cell ss:Index="8" ss:MergeDown="2" ss:StyleID="s467"><Data ss:Type="String">кол-во, шт</Data></Cell>
				<Cell ss:MergeDown="2" ss:StyleID="s467"><Data ss:Type="String">дата ввода</Data></Cell>

				<Cell ss:Index="41" ss:MergeDown="2" ss:StyleID="s467"><Data ss:Type="String">кол-во потребителей</Data></Cell>
				<Cell  ss:MergeDown="2" ss:StyleID="s467"><Data ss:Type="String">стоки</Data></Cell>
			</Row>
			<Row ss:Height="12.75">
				
				
			</Row>
			<Row ss:Height="50.75">
				<Cell ss:Index="11"    ss:StyleID="s467"><Data ss:Type="String">холодное водоснабжение</Data></Cell>
				<Cell ss:StyleID="s467"><Data ss:Type="String">горячее водоснабжение централизованное</Data></Cell>
				<Cell ss:StyleID="s467"><Data ss:Type="String">горячее водоснабжение из открытой системы ТС</Data></Cell>
				<Cell ss:StyleID="s467"><Data ss:Type="String">теплообменники</Data></Cell>
				<Cell ss:StyleID="s467"><Data ss:Type="String">холодное водоснабжение</Data></Cell>
				<Cell ss:StyleID="s467"><Data ss:Type="String">горячее водоснабжение централизованное</Data></Cell>
				<Cell ss:StyleID="s467"><Data ss:Type="String">горячее водоснабжение из открытой системы ТС</Data></Cell>
				<Cell ss:StyleID="s467"><Data ss:Type="String">теплообменники</Data></Cell>


				<Cell ss:Index="22"    ss:StyleID="s467"><Data ss:Type="String">холодное водоснабжение</Data></Cell>
				<Cell ss:StyleID="s467"><Data ss:Type="String">горячее водоснабжение централизованное</Data></Cell>
				<Cell ss:StyleID="s467"><Data ss:Type="String">горячее водоснабжение из открытой системы ТС</Data></Cell>
				<Cell ss:StyleID="s467"><Data ss:Type="String">теплообменники</Data></Cell>
				<Cell ss:StyleID="s467"><Data ss:Type="String">холодное водоснабжение</Data></Cell>
				<Cell ss:StyleID="s467"><Data ss:Type="String">горячее водоснабжение централизованное</Data></Cell>
				<Cell ss:StyleID="s467"><Data ss:Type="String">горячее водоснабжение из открытой системы ТС</Data></Cell>
				<Cell ss:StyleID="s467"><Data ss:Type="String">теплообменники</Data></Cell>

				<Cell ss:Index="33"    ss:StyleID="s467"><Data ss:Type="String">холодное водоснабжение</Data></Cell>
				<Cell ss:StyleID="s467"><Data ss:Type="String">горячее водоснабжение централизованное</Data></Cell>
				<Cell ss:StyleID="s467"><Data ss:Type="String">горячее водоснабжение из открытой системы ТС</Data></Cell>
				<Cell ss:StyleID="s467"><Data ss:Type="String">теплообменники</Data></Cell>
				<Cell ss:StyleID="s467"><Data ss:Type="String">холодное водоснабжение</Data></Cell>
				<Cell ss:StyleID="s467"><Data ss:Type="String">горячее водоснабжение централизованное</Data></Cell>
				<Cell ss:StyleID="s467"><Data ss:Type="String">горячее водоснабжение из открытой системы ТС</Data></Cell>
				<Cell ss:StyleID="s467"><Data ss:Type="String">теплообменники</Data></Cell>
			</Row>
			
       
            <Row ss:Height="12.75">
              <Cell ss:StyleID="s412"><Data ss:Type="Number">1</Data></Cell>
              <Cell ss:StyleID="s412"><Data ss:Type="Number">2</Data></Cell>
			  <Cell ss:StyleID="s412"/>
			  <Cell ss:StyleID="s412"/>
              <Cell ss:StyleID="s412"><Data ss:Type="Number">3</Data></Cell>
              <Cell ss:StyleID="s412"><Data ss:Type="Number">4</Data></Cell>
              <Cell ss:StyleID="s412"><Data ss:Type="Number">5</Data></Cell>
              <Cell ss:StyleID="s412"><Data ss:Type="Number">6</Data></Cell>
              <Cell ss:StyleID="s412"><Data ss:Type="Number">7</Data></Cell>
              <Cell ss:StyleID="s412"><Data ss:Type="Number">8</Data></Cell>
              <Cell ss:StyleID="s412"><Data ss:Type="Number">9</Data></Cell>
              <Cell ss:StyleID="s412"><Data ss:Type="Number">10</Data></Cell>
              <Cell ss:StyleID="s412"><Data ss:Type="Number">11</Data></Cell>
              <Cell ss:StyleID="s412"><Data ss:Type="Number">12</Data></Cell>
              <Cell ss:StyleID="s412"><Data ss:Type="Number">13</Data></Cell>
              <Cell ss:StyleID="s412"><Data ss:Type="Number">14</Data></Cell>
              <Cell ss:StyleID="s412"><Data ss:Type="Number">15</Data></Cell>
              <Cell ss:StyleID="s412"><Data ss:Type="Number">16</Data></Cell>
              <Cell ss:StyleID="s412"><Data ss:Type="Number">17</Data></Cell>
              <Cell ss:StyleID="s412"><Data ss:Type="Number">18</Data></Cell>
              <Cell ss:StyleID="s412"><Data ss:Type="Number">19</Data></Cell>
              <Cell ss:StyleID="s412"><Data ss:Type="Number">20</Data></Cell>
              <Cell ss:StyleID="s412"><Data ss:Type="Number">21</Data></Cell>
              <Cell ss:StyleID="s412"><Data ss:Type="Number">22</Data></Cell>
              <Cell ss:StyleID="s412"><Data ss:Type="Number">23</Data></Cell>
              <Cell ss:StyleID="s412"><Data ss:Type="Number">24</Data></Cell>
              <Cell ss:StyleID="s412"><Data ss:Type="Number">25</Data></Cell>
              <Cell ss:StyleID="s412"><Data ss:Type="Number">26</Data></Cell>
              <Cell ss:StyleID="s412"><Data ss:Type="Number">27</Data></Cell>
              <Cell ss:StyleID="s412"><Data ss:Type="Number">28</Data></Cell>
              <Cell ss:StyleID="s412"><Data ss:Type="Number">29</Data></Cell>
              <Cell ss:StyleID="s412"><Data ss:Type="Number">30</Data></Cell>
              <Cell ss:StyleID="s412"><Data ss:Type="Number">31</Data></Cell>
              <Cell ss:StyleID="s412"><Data ss:Type="Number">32</Data></Cell>
              <Cell ss:StyleID="s412"><Data ss:Type="Number">33</Data></Cell>
              <Cell ss:StyleID="s412"><Data ss:Type="Number">34</Data></Cell>
              <Cell ss:StyleID="s412"><Data ss:Type="Number">35</Data></Cell>
              <Cell ss:StyleID="s412"><Data ss:Type="Number">36</Data></Cell>
              <Cell ss:StyleID="s412"><Data ss:Type="Number">37</Data></Cell>
              <Cell ss:StyleID="s412"><Data ss:Type="Number">38</Data></Cell>
              <Cell ss:StyleID="s412"><Data ss:Type="Number">39</Data></Cell>
              <Cell ss:StyleID="s412"><Data ss:Type="Number">40</Data></Cell>
              <Cell ss:StyleID="s412"><Data ss:Type="Number">41</Data></Cell>
              <Cell ss:StyleID="s412"><Data ss:Type="Number">42</Data></Cell>
              <Cell ss:StyleID="s412"><Data ss:Type="Number">43</Data></Cell>
              <Cell ss:StyleID="s412"><Data ss:Type="Number">44</Data></Cell>
            </Row>
            <!-- <<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<< Данные таблицы >>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>> -->
            <xsl:apply-templates select="//TS11" />
          </Table>
          <WorksheetOptions xmlns="urn:schemas-microsoft-com:office:excel">
            <Zoom>80</Zoom>
          </WorksheetOptions>
        </Worksheet>
      
      </xsl:when>
      
      <!-- <<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<< Лист ТХ31 >>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>> -->
      <xsl:when test="TS31 = 1">
        <Worksheet ss:Name="ВО3">
          <Table>
            <Column ss:AutoFitWidth="0" ss:Width="46.5"/>
            <Column ss:AutoFitWidth="0" ss:Width="1000"/>
            <Column ss:AutoFitWidth="0" ss:Width="127.5"/>
            <Column ss:AutoFitWidth="0" ss:Width="124.5"/>
            <Column ss:AutoFitWidth="0" ss:Width="76.5"/>
            <Column ss:AutoFitWidth="0" ss:Width="78"/>
            <Column ss:AutoFitWidth="0" ss:Width="60"/>
            <Column ss:AutoFitWidth="0" ss:Width="48.75" ss:Span="2"/>
            <Column ss:Index="11" ss:AutoFitWidth="1" ss:Width="54"/>
            <Column ss:AutoFitWidth="1" ss:Width="48.75" ss:Span="45"/>
            <Column ss:Index="58" ss:AutoFitWidth="0" ss:Width="76.5"/>
            
            <Row ss:Height="12.75">
              <Cell ss:Index="2" ss:StyleID="s395"><Data ss:Type="String">Потребители системы водоотведения</Data></Cell>
            </Row>
            <Row ss:Height="12.75">
              <Cell ss:Index="2" ss:StyleID="s402"><ss:Data ss:Type="String" xmlns="http://www.w3.org/TR/REC-html40"><B>по предприятию <U>___</U></B><I><U><Font html:Color="#FF0000">(указать наименование организации)</Font></U></I><B><I><U>_</U></I><U>_</U></B></ss:Data></Cell>
            </Row>
            <Row ss:Height="12.75">
              <Cell ss:Index="2" ss:StyleID="s402"><ss:Data ss:Type="String" xmlns="http://www.w3.org/TR/REC-html40"><B>на _</B><I><U><Font html:Color="#FF0000">(указать регулируемый период)</Font></U></I><B>_ год</B></ss:Data></Cell>
            </Row>
            <Row ss:Height="12.75"/>
			
            <Row ss:Height="12.75">
              <Cell ss:MergeDown="6" ss:StyleID="s459"><Data ss:Type="String">№ п/п</Data></Cell>
              <Cell ss:MergeDown="6" ss:StyleID="s490"><Data ss:Type="String">Категория потребителя</Data></Cell>
              <Cell ss:MergeDown="6" ss:StyleID="m201520164"><Data ss:Type="String">Район</Data></Cell>
              <Cell ss:MergeDown="6" ss:StyleID="m201520184"><Data ss:Type="String">Наслег</Data></Cell>
              <Cell ss:MergeDown="6" ss:StyleID="s459"><Data ss:Type="String">населенного пункта</Data></Cell>
              <Cell ss:MergeDown="6" ss:StyleID="s459"><Data ss:Type="String">Наименование котельной</Data></Cell>
              <Cell ss:MergeDown="6" ss:StyleID="s490"><Data ss:Type="String">Идентиф. код  котельной</Data></Cell>
              <Cell ss:MergeAcross="1" ss:MergeDown="3" ss:StyleID="s459"><Data ss:Type="String">Вновь вводимые объекты</Data></Cell>
              <Cell ss:MergeAcross="8" ss:MergeDown="1" ss:StyleID="s479"><Data ss:Type="String">Нагрузка (мощность) по договорам водоотведения, м3/час</Data></Cell>
              <Cell ss:MergeAcross="10" ss:MergeDown="1" ss:StyleID="s480"><Data ss:Type="String">Утвержденный в тарифе объем в отчетном периоде</Data></Cell>
              <Cell ss:MergeAcross="10" ss:MergeDown="1" ss:StyleID="s467"><ss:Data ss:Type="String" xmlns="http://www.w3.org/TR/REC-html40">Плановый объем потребления на регулируемый период</ss:Data></Cell>
              <Cell ss:MergeAcross="1" ss:MergeDown="3" ss:StyleID="s467"><Data ss:Type="String">Отклонение плана от тарифа, %</Data></Cell>
              <Cell ss:MergeDown="6" ss:StyleID="s467"><Data ss:Type="String">Фактическое потребление, по приборам учета, м3</Data></Cell>
              <Cell ss:MergeDown="6" ss:StyleID="s467"><Data ss:Type="String">Наличие приборов учета, шт</Data></Cell>
              <Cell ss:MergeDown="6" ss:StyleID="s467"><Data ss:Type="String">Процент установки приборов учета, %</Data></Cell>
              <Cell ss:MergeDown="6" ss:StyleID="s467"><Data ss:Type="String">Причины изменения</Data></Cell>
            
            </Row>
            <Row ss:Height="12.75"/>
            <Row ss:Height="12.75">
			 <Cell ss:Index="10" ss:MergeDown="4" ss:StyleID="s467"><Data ss:Type="String">Всего, М3/час</Data></Cell>
			 <Cell ss:MergeAcross="7" ss:StyleID="s467"><Data ss:Type="String">в том числе</Data></Cell>
			 <Cell ss:MergeDown="4" ss:StyleID="s467"><Data ss:Type="String">Степень благоустройства</Data></Cell>
			 <Cell ss:MergeDown="4" ss:StyleID="s467"><Data ss:Type="String">Количество потребителей, чел</Data></Cell>
			 <Cell ss:MergeDown="4" ss:StyleID="s467"><Data ss:Type="String">Всего, м3</Data></Cell>
			 <Cell ss:MergeAcross="7" ss:StyleID="s467"><Data ss:Type="String">в том числе</Data></Cell>
			 <Cell ss:MergeDown="4" ss:StyleID="s467"><Data ss:Type="String">Степень благоустройства</Data></Cell>
			 <Cell ss:MergeDown="4" ss:StyleID="s467"><Data ss:Type="String">Количество потребителей, чел</Data></Cell>
			 <Cell ss:MergeDown="4" ss:StyleID="s467"><Data ss:Type="String">Всего, м3</Data></Cell>
			 <Cell ss:MergeAcross="7" ss:StyleID="s467"><Data ss:Type="String">в том числе</Data></Cell>
			</Row>
			<Row ss:Height="12.75">
				<Cell ss:Index="11" ss:MergeAcross="3" ss:MergeDown="2" ss:StyleID="s467"><Data ss:Type="String">коллекторная канализация</Data></Cell>
				<Cell  ss:MergeAcross="3" ss:MergeDown="2" ss:StyleID="s467"><Data ss:Type="String">вывозная канализация</Data></Cell>

				<Cell ss:Index="22" ss:MergeAcross="3" ss:MergeDown="2" ss:StyleID="s467"><Data ss:Type="String">коллекторная канализация</Data></Cell>
				<Cell  ss:MergeAcross="3" ss:MergeDown="2" ss:StyleID="s467"><Data ss:Type="String">вывозная канализация</Data></Cell>

				<Cell ss:Index="33" ss:MergeAcross="3" ss:MergeDown="2" ss:StyleID="s467"><Data ss:Type="String">коллекторная канализация</Data></Cell>
				<Cell  ss:MergeAcross="3" ss:MergeDown="2" ss:StyleID="s467"><Data ss:Type="String">вывозная канализация</Data></Cell>
			</Row>
			<Row ss:Height="12.75">
				<Cell ss:Index="8" ss:MergeDown="2" ss:StyleID="s467"><Data ss:Type="String">кол-во, шт</Data></Cell>
				<Cell ss:MergeDown="2" ss:StyleID="s467"><Data ss:Type="String">дата ввода</Data></Cell>

				<Cell ss:Index="41" ss:MergeDown="2" ss:StyleID="s467"><Data ss:Type="String">кол-во потребителей</Data></Cell>
				<Cell  ss:MergeDown="2" ss:StyleID="s467"><Data ss:Type="String">стоки</Data></Cell>
			</Row>
			<Row ss:Height="12.75">
				
				
			</Row>
			<Row ss:Height="50.75">
				<Cell ss:Index="11"    ss:StyleID="s467"><Data ss:Type="String">холодное водоснабжение</Data></Cell>
				<Cell ss:StyleID="s467"><Data ss:Type="String">горячее водоснабжение централизованное</Data></Cell>
				<Cell ss:StyleID="s467"><Data ss:Type="String">горячее водоснабжение из открытой системы ТС</Data></Cell>
				<Cell ss:StyleID="s467"><Data ss:Type="String">теплообменники</Data></Cell>
				<Cell ss:StyleID="s467"><Data ss:Type="String">холодное водоснабжение</Data></Cell>
				<Cell ss:StyleID="s467"><Data ss:Type="String">горячее водоснабжение централизованное</Data></Cell>
				<Cell ss:StyleID="s467"><Data ss:Type="String">горячее водоснабжение из открытой системы ТС</Data></Cell>
				<Cell ss:StyleID="s467"><Data ss:Type="String">теплообменники</Data></Cell>


				<Cell ss:Index="22"    ss:StyleID="s467"><Data ss:Type="String">холодное водоснабжение</Data></Cell>
				<Cell ss:StyleID="s467"><Data ss:Type="String">горячее водоснабжение централизованное</Data></Cell>
				<Cell ss:StyleID="s467"><Data ss:Type="String">горячее водоснабжение из открытой системы ТС</Data></Cell>
				<Cell ss:StyleID="s467"><Data ss:Type="String">теплообменники</Data></Cell>
				<Cell ss:StyleID="s467"><Data ss:Type="String">холодное водоснабжение</Data></Cell>
				<Cell ss:StyleID="s467"><Data ss:Type="String">горячее водоснабжение централизованное</Data></Cell>
				<Cell ss:StyleID="s467"><Data ss:Type="String">горячее водоснабжение из открытой системы ТС</Data></Cell>
				<Cell ss:StyleID="s467"><Data ss:Type="String">теплообменники</Data></Cell>

				<Cell ss:Index="33"    ss:StyleID="s467"><Data ss:Type="String">холодное водоснабжение</Data></Cell>
				<Cell ss:StyleID="s467"><Data ss:Type="String">горячее водоснабжение централизованное</Data></Cell>
				<Cell ss:StyleID="s467"><Data ss:Type="String">горячее водоснабжение из открытой системы ТС</Data></Cell>
				<Cell ss:StyleID="s467"><Data ss:Type="String">теплообменники</Data></Cell>
				<Cell ss:StyleID="s467"><Data ss:Type="String">холодное водоснабжение</Data></Cell>
				<Cell ss:StyleID="s467"><Data ss:Type="String">горячее водоснабжение централизованное</Data></Cell>
				<Cell ss:StyleID="s467"><Data ss:Type="String">горячее водоснабжение из открытой системы ТС</Data></Cell>
				<Cell ss:StyleID="s467"><Data ss:Type="String">теплообменники</Data></Cell>
			</Row>
			
       
            <Row ss:Height="12.75">
              <Cell ss:StyleID="s412"><Data ss:Type="Number">1</Data></Cell>
              <Cell ss:StyleID="s412"><Data ss:Type="Number">2</Data></Cell>
			  <Cell ss:StyleID="s412"/>
			  <Cell ss:StyleID="s412"/>
              <Cell ss:StyleID="s412"><Data ss:Type="Number">3</Data></Cell>
              <Cell ss:StyleID="s412"><Data ss:Type="Number">4</Data></Cell>
              <Cell ss:StyleID="s412"><Data ss:Type="Number">5</Data></Cell>
              <Cell ss:StyleID="s412"><Data ss:Type="Number">6</Data></Cell>
              <Cell ss:StyleID="s412"><Data ss:Type="Number">7</Data></Cell>
              <Cell ss:StyleID="s412"><Data ss:Type="Number">8</Data></Cell>
              <Cell ss:StyleID="s412"><Data ss:Type="Number">9</Data></Cell>
              <Cell ss:StyleID="s412"><Data ss:Type="Number">10</Data></Cell>
              <Cell ss:StyleID="s412"><Data ss:Type="Number">11</Data></Cell>
              <Cell ss:StyleID="s412"><Data ss:Type="Number">12</Data></Cell>
              <Cell ss:StyleID="s412"><Data ss:Type="Number">13</Data></Cell>
              <Cell ss:StyleID="s412"><Data ss:Type="Number">14</Data></Cell>
              <Cell ss:StyleID="s412"><Data ss:Type="Number">15</Data></Cell>
              <Cell ss:StyleID="s412"><Data ss:Type="Number">16</Data></Cell>
              <Cell ss:StyleID="s412"><Data ss:Type="Number">17</Data></Cell>
              <Cell ss:StyleID="s412"><Data ss:Type="Number">18</Data></Cell>
              <Cell ss:StyleID="s412"><Data ss:Type="Number">19</Data></Cell>
              <Cell ss:StyleID="s412"><Data ss:Type="Number">20</Data></Cell>
              <Cell ss:StyleID="s412"><Data ss:Type="Number">21</Data></Cell>
              <Cell ss:StyleID="s412"><Data ss:Type="Number">22</Data></Cell>
              <Cell ss:StyleID="s412"><Data ss:Type="Number">23</Data></Cell>
              <Cell ss:StyleID="s412"><Data ss:Type="Number">24</Data></Cell>
              <Cell ss:StyleID="s412"><Data ss:Type="Number">25</Data></Cell>
              <Cell ss:StyleID="s412"><Data ss:Type="Number">26</Data></Cell>
              <Cell ss:StyleID="s412"><Data ss:Type="Number">27</Data></Cell>
              <Cell ss:StyleID="s412"><Data ss:Type="Number">28</Data></Cell>
              <Cell ss:StyleID="s412"><Data ss:Type="Number">29</Data></Cell>
              <Cell ss:StyleID="s412"><Data ss:Type="Number">30</Data></Cell>
              <Cell ss:StyleID="s412"><Data ss:Type="Number">31</Data></Cell>
              <Cell ss:StyleID="s412"><Data ss:Type="Number">32</Data></Cell>
              <Cell ss:StyleID="s412"><Data ss:Type="Number">33</Data></Cell>
              <Cell ss:StyleID="s412"><Data ss:Type="Number">34</Data></Cell>
              <Cell ss:StyleID="s412"><Data ss:Type="Number">35</Data></Cell>
              <Cell ss:StyleID="s412"><Data ss:Type="Number">36</Data></Cell>
              <Cell ss:StyleID="s412"><Data ss:Type="Number">37</Data></Cell>
              <Cell ss:StyleID="s412"><Data ss:Type="Number">38</Data></Cell>
              <Cell ss:StyleID="s412"><Data ss:Type="Number">39</Data></Cell>
              <Cell ss:StyleID="s412"><Data ss:Type="Number">40</Data></Cell>
              <Cell ss:StyleID="s412"><Data ss:Type="Number">41</Data></Cell>
              <Cell ss:StyleID="s412"><Data ss:Type="Number">42</Data></Cell>
              <Cell ss:StyleID="s412"><Data ss:Type="Number">43</Data></Cell>
              <Cell ss:StyleID="s412"><Data ss:Type="Number">44</Data></Cell>
            </Row>
            <!-- <<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<< Данные таблицы >>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>> -->
            <xsl:apply-templates select="//TS31" />
          </Table>
          <WorksheetOptions xmlns="urn:schemas-microsoft-com:office:excel">
            <Zoom>80</Zoom>
          </WorksheetOptions>
        </Worksheet>
      
      </xsl:when>
        
      <xsl:when test="TS1 = 1">
        <!-- <<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<< Лист ТХ1 >>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>> -->
        <Worksheet ss:Name="ТХ1">
          <Table>
            <Column ss:AutoFitWidth="0" ss:Width="46.5"/>
            <Column ss:AutoFitWidth="0" ss:Width="1000"/>
            <Column ss:AutoFitWidth="0" ss:Width="127.5"/>
            <Column ss:AutoFitWidth="0" ss:Width="124.5"/>
            <Column ss:AutoFitWidth="0" ss:Width="76.5"/>
            <Column ss:AutoFitWidth="0" ss:Width="78"/>
            <Column ss:AutoFitWidth="0" ss:Width="60"/>
            <Column ss:AutoFitWidth="0" ss:Width="48.75" ss:Span="2"/>
            <Column ss:Index="11" ss:AutoFitWidth="1" ss:Width="54"/>
            <Column ss:AutoFitWidth="1" ss:Width="48.75" ss:Span="45"/>
            <Column ss:Index="58" ss:AutoFitWidth="0" ss:Width="76.5"/>
            <Row ss:Height="12.75">
              <Cell ss:Index="2" ss:StyleID="s395"><Data ss:Type="String">Потребление тепловой энергии</Data></Cell>
            </Row>
            <Row ss:Height="12.75">
              <Cell ss:Index="2" ss:StyleID="s402"><ss:Data ss:Type="String" xmlns="http://www.w3.org/TR/REC-html40"><B>по предприятию <U>___</U></B><I><U><Font html:Color="#FF0000">(указать наименование организации)</Font></U></I><B><I><U>_</U></I><U>_</U></B></ss:Data></Cell>
            </Row>
            <Row ss:Height="12.75">
              <Cell ss:Index="2" ss:StyleID="s402"><ss:Data ss:Type="String" xmlns="http://www.w3.org/TR/REC-html40"><B>на _</B><I><U><Font html:Color="#FF0000">(указать регулируемый период)</Font></U></I><B>_ год</B></ss:Data></Cell>
            </Row>
            <Row ss:Height="12.75"/>
            <Row ss:Height="12.75">
              <Cell ss:MergeDown="5" ss:StyleID="s459"><Data ss:Type="String">№ п/п</Data></Cell>
              <Cell ss:MergeDown="5" ss:StyleID="s490"><Data ss:Type="String">Категория потребителя</Data></Cell>
              <Cell ss:MergeDown="5" ss:StyleID="m201520164"><Data ss:Type="String">Район</Data></Cell>
              <Cell ss:MergeDown="5" ss:StyleID="m201520184"><Data ss:Type="String">Наслег</Data></Cell>
              <Cell ss:MergeDown="5" ss:StyleID="s459"><Data ss:Type="String">населенного пункта</Data></Cell>
              <Cell ss:MergeDown="5" ss:StyleID="s459"><Data ss:Type="String">Наименование котельной</Data></Cell>
              <Cell ss:MergeDown="5" ss:StyleID="s490"><Data ss:Type="String">Идентиф. код  котельной</Data></Cell>
              <Cell ss:MergeAcross="1" ss:MergeDown="3" ss:StyleID="s459"><Data ss:Type="String">Вновь вводимые объекты</Data></Cell>
              <Cell ss:MergeDown="5" ss:StyleID="s479"><Data ss:Type="String">Степень благоустройства</Data></Cell>
              <Cell ss:MergeDown="5" ss:StyleID="s480"><Data ss:Type="String">Продолж. отоп. периода, сут.</Data></Cell>
              <Cell ss:MergeAcross="3" ss:MergeDown="1" ss:StyleID="s469"><ss:Data ss:Type="String" xmlns="http://www.w3.org/TR/REC-html40">Тепловая нагрузка (мощность) по договорам теплоснабжения, Гкал/час</ss:Data></Cell>
              <Cell ss:MergeAcross="13" ss:MergeDown="1" ss:StyleID="s482"><Data ss:Type="String">Утвержденные в тарифе объемы тепла в отчетном периоде</Data></Cell>
              <Cell ss:MergeAcross="13" ss:MergeDown="1" ss:StyleID="s483"><Data ss:Type="String">Прогноз потребления тепла на регулируемый период</Data></Cell>
              <Cell ss:MergeAcross="13" ss:MergeDown="1" ss:StyleID="s478"><Data ss:Type="String">Отклонение прогноза от тарифа</Data></Cell>
              <Cell ss:MergeDown="5" ss:StyleID="s467"><Data ss:Type="String">Причины изменения</Data></Cell>
            </Row>
            <Row ss:Height="12.75"/>
            <Row ss:Height="12.75">
              <Cell ss:Index="12" ss:MergeDown="3" ss:StyleID="s469"><Data ss:Type="String">Всего, Гкал/час</Data></Cell>
              <Cell ss:MergeAcross="2" ss:StyleID="s460"><Data ss:Type="String">в том числе</Data></Cell>
              <Cell ss:MergeDown="3" ss:StyleID="s470"><Data ss:Type="String">Этажность</Data></Cell>
              <Cell ss:MergeDown="3" ss:StyleID="m201519504"><Data ss:Type="String">Степень благоустройства</Data></Cell>
              <Cell ss:MergeDown="3" ss:StyleID="m201519524"><Data ss:Type="String">Количество потребителей, чел</Data></Cell>
              <Cell ss:MergeDown="3" ss:StyleID="m201519544"><Data ss:Type="String">Остекление окон (двойное-2/тройное-3)</Data></Cell>
              <Cell ss:MergeAcross="1" ss:MergeDown="2" ss:StyleID="s475"><Data ss:Type="String">Отапливаемая (ый) площадь (объем)</Data></Cell>
              <Cell ss:MergeDown="3" ss:StyleID="s475"><Data ss:Type="String">Всего, Гкал</Data></Cell>
              <Cell ss:MergeAcross="6" ss:StyleID="s477"><Data ss:Type="String">в том числе</Data></Cell>
              <Cell ss:MergeDown="3" ss:StyleID="s455"><Data ss:Type="String">Этажность</Data></Cell>
              <Cell ss:MergeDown="3" ss:StyleID="m201519784"><Data ss:Type="String">Степень благоустройства</Data></Cell>
              <Cell ss:MergeDown="3" ss:StyleID="m201519804"><Data ss:Type="String">Количество потребителей, чел</Data></Cell>
              <Cell ss:MergeDown="3" ss:StyleID="m201519824"><Data ss:Type="String">Остекление окон (двойное-2/тройное-3)</Data></Cell>
              <Cell ss:MergeAcross="1" ss:MergeDown="2" ss:StyleID="s455"><Data ss:Type="String">Отапливаемая (ый) площадь (объем)</Data></Cell>
              <Cell ss:MergeDown="3" ss:StyleID="s455"><Data ss:Type="String">Всего, Гкал</Data></Cell>
              <Cell ss:MergeAcross="6" ss:StyleID="s457"><Data ss:Type="String">в том числе</Data></Cell>
              <Cell ss:MergeDown="3" ss:StyleID="s411"><Data ss:Type="String">Этажность</Data></Cell>
              <Cell ss:MergeDown="3" ss:StyleID="m201519664"><Data ss:Type="String">Степень благоустройства</Data></Cell>
              <Cell ss:MergeDown="3" ss:StyleID="m201519684"><Data ss:Type="String">Количество потребителей, чел</Data></Cell>
              <Cell ss:MergeDown="3" ss:StyleID="m143752700"><Data ss:Type="String">Остекление окон (двойное-2/тройное-3)</Data></Cell>
              <Cell ss:MergeAcross="1" ss:MergeDown="2" ss:StyleID="s411"><Data ss:Type="String">Отапливаемая (ый) площадь (объем)</Data></Cell>
              <Cell ss:MergeDown="3" ss:StyleID="s411"><Data ss:Type="String">Всего, Гкал</Data></Cell>
              <Cell ss:MergeAcross="6" ss:StyleID="s466"><Data ss:Type="String">в том числе</Data></Cell>
            </Row>
            <Row ss:Height="12.75">
              <Cell ss:Index="13" ss:MergeDown="2" ss:StyleID="s460"><Data ss:Type="String">на теплоснабжение</Data></Cell>
              <Cell ss:MergeAcross="1" ss:StyleID="s487"><Data ss:Type="String">на горячее водоснабжение</Data></Cell>
              <Cell ss:Index="23" ss:MergeAcross="3" ss:StyleID="s475"><Data ss:Type="String">на теплоснабжение</Data></Cell>
              <Cell ss:MergeAcross="2" ss:StyleID="s488"><Data ss:Type="String">на горячее водоснабжение</Data></Cell>
              <Cell ss:Index="37" ss:MergeAcross="3" ss:StyleID="s455"><Data ss:Type="String">на теплоснабжение</Data></Cell>
              <Cell ss:MergeAcross="2" ss:StyleID="s489"><Data ss:Type="String">на горячее водоснабжение</Data></Cell>
              <Cell ss:Index="51" ss:MergeAcross="3" ss:StyleID="s411"><Data ss:Type="String">на теплоснабжение</Data></Cell>
              <Cell ss:MergeAcross="2" ss:StyleID="s458"><Data ss:Type="String">на горячее водоснабжение</Data></Cell>
            </Row>
            <Row ss:Height="12.75">
              <Cell ss:Index="8" ss:MergeDown="1" ss:StyleID="s459"><Data ss:Type="String">кол-во, шт</Data></Cell>
              <Cell ss:MergeDown="1" ss:StyleID="s459"><Data ss:Type="String">дата ввода</Data></Cell>
              <Cell ss:Index="14" ss:MergeDown="1" ss:StyleID="s460"><Data ss:Type="String">из центр. системы ГВС</Data></Cell>
              <Cell ss:MergeDown="1" ss:StyleID="s460"><Data ss:Type="String">из системы отопления</Data></Cell>
              <Cell ss:Index="23" ss:MergeDown="1" ss:StyleID="s409"><Data ss:Type="String">на отопление</Data></Cell>
              <Cell ss:MergeDown="1" ss:StyleID="s409"><Data ss:Type="String">на вентиляцию</Data></Cell>
              <Cell ss:MergeDown="1" ss:StyleID="s409"><Data ss:Type="String">спутники ТС</Data></Cell>
              <Cell ss:MergeDown="1" ss:StyleID="s409"><Data ss:Type="String">потери в абон.сетях ТС</Data></Cell>
              <Cell ss:MergeDown="1" ss:StyleID="s409"><Data ss:Type="String"> из центр. системы ГВС</Data></Cell>
              <Cell ss:MergeDown="1" ss:StyleID="s409"><Data ss:Type="String">из системы отопления</Data></Cell>
              <Cell ss:MergeDown="1" ss:StyleID="s409"><Data ss:Type="String">теплообменники</Data></Cell>
              <Cell ss:Index="37" ss:MergeDown="1" ss:StyleID="s410"><Data ss:Type="String">на отопление</Data></Cell>
              <Cell ss:MergeDown="1" ss:StyleID="s410"><Data ss:Type="String">на вентиляцию</Data></Cell>
              <Cell ss:MergeDown="1" ss:StyleID="s410"><Data ss:Type="String">спутники ТС</Data></Cell>
              <Cell ss:MergeDown="1" ss:StyleID="s410"><Data ss:Type="String">потери в сетях ТС</Data></Cell>
              <Cell ss:MergeDown="1" ss:StyleID="s410"><Data ss:Type="String"> из центр. системы ГВС</Data></Cell>
              <Cell ss:MergeDown="1" ss:StyleID="s410"><Data ss:Type="String">из системы отопления</Data></Cell>
              <Cell ss:MergeDown="1" ss:StyleID="s410"><Data ss:Type="String">теплообменники</Data></Cell>
              <Cell ss:Index="51" ss:MergeDown="1" ss:StyleID="s411"><Data ss:Type="String">на отопление</Data></Cell>
              <Cell ss:MergeDown="1" ss:StyleID="s411"><Data ss:Type="String">на вентиляцию</Data></Cell>
              <Cell ss:MergeDown="1" ss:StyleID="s411"><Data ss:Type="String">спутники ТС</Data></Cell>
              <Cell ss:MergeDown="1" ss:StyleID="s411"><Data ss:Type="String">потери в сетях ТС</Data></Cell>
              <Cell ss:MergeDown="1" ss:StyleID="s411"><Data ss:Type="String"> из центр. системы ГВС</Data></Cell>
              <Cell ss:MergeDown="1" ss:StyleID="s411"><Data ss:Type="String">из системы отопления</Data></Cell>
              <Cell ss:MergeDown="1" ss:StyleID="s411"><Data ss:Type="String">теплообменники</Data></Cell>
            </Row>
            <Row ss:Height="28.5">
              <Cell ss:Index="20" ss:StyleID="s409"><Data ss:Type="String">м2</Data></Cell>
              <Cell ss:StyleID="s409"><Data ss:Type="String">м3</Data></Cell>
              <Cell ss:Index="34" ss:StyleID="s410"><Data ss:Type="String">м2</Data></Cell>
              <Cell ss:StyleID="s410"><Data ss:Type="String">м3</Data></Cell>
              <Cell ss:Index="48" ss:StyleID="s411"><Data ss:Type="String">м2</Data></Cell>
              <Cell ss:StyleID="s411"><Data ss:Type="String">м3</Data></Cell>
            </Row>
            <Row ss:Height="12.75">
              <Cell ss:StyleID="s412"><Data ss:Type="Number">1</Data></Cell>
              <Cell ss:StyleID="s412"><Data ss:Type="Number">2</Data></Cell>
              <Cell ss:StyleID="s412"/>
              <Cell ss:StyleID="s412"/>
              <Cell ss:StyleID="s412"><Data ss:Type="Number">3</Data></Cell>
              <Cell ss:StyleID="s412"><Data ss:Type="Number">4</Data></Cell>
              <Cell ss:StyleID="s412"><Data ss:Type="Number">5</Data></Cell>
              <Cell ss:StyleID="s412"><Data ss:Type="Number">6</Data></Cell>
              <Cell ss:StyleID="s412"><Data ss:Type="Number">7</Data></Cell>
              <Cell ss:StyleID="s412"><Data ss:Type="Number">8</Data></Cell>
              <Cell ss:StyleID="s412"><Data ss:Type="Number">9</Data></Cell>
              <Cell ss:StyleID="s412"><Data ss:Type="Number">10</Data></Cell>
              <Cell ss:StyleID="s412"><Data ss:Type="Number">11</Data></Cell>
              <Cell ss:StyleID="s412"><Data ss:Type="Number">12</Data></Cell>
              <Cell ss:StyleID="s412"><Data ss:Type="Number">13</Data></Cell>
              <Cell ss:StyleID="s412"><Data ss:Type="Number">14</Data></Cell>
              <Cell ss:StyleID="s412"/>
              <Cell ss:StyleID="s412"/>
              <Cell ss:StyleID="s412"/>
              <Cell ss:StyleID="s412"><Data ss:Type="Number">15</Data></Cell>
              <Cell ss:StyleID="s412"><Data ss:Type="Number">16</Data></Cell>
              <Cell ss:StyleID="s412"><Data ss:Type="Number">17</Data></Cell>
              <Cell ss:StyleID="s412"><Data ss:Type="Number">18</Data></Cell>
              <Cell ss:StyleID="s412"><Data ss:Type="Number">19</Data></Cell>
              <Cell ss:StyleID="s412"><Data ss:Type="Number">20</Data></Cell>
              <Cell ss:StyleID="s412"><Data ss:Type="Number">21</Data></Cell>
              <Cell ss:StyleID="s412"><Data ss:Type="Number">22</Data></Cell>
              <Cell ss:StyleID="s412"><Data ss:Type="Number">23</Data></Cell>
              <Cell ss:StyleID="s412"><Data ss:Type="Number">24</Data></Cell>
              <Cell ss:StyleID="s412"><Data ss:Type="Number">25</Data></Cell>
              <Cell ss:StyleID="s412"/>
              <Cell ss:StyleID="s412"/>
              <Cell ss:StyleID="s412"/>
              <Cell ss:StyleID="s412"><Data ss:Type="Number">26</Data></Cell>
              <Cell ss:StyleID="s412"><Data ss:Type="Number">27</Data></Cell>
              <Cell ss:StyleID="s412"><Data ss:Type="Number">28</Data></Cell>
              <Cell ss:StyleID="s412"><Data ss:Type="Number">29</Data></Cell>
              <Cell ss:StyleID="s412"><Data ss:Type="Number">30</Data></Cell>
              <Cell ss:StyleID="s412"><Data ss:Type="Number">31</Data></Cell>
              <Cell ss:StyleID="s412"><Data ss:Type="Number">32</Data></Cell>
              <Cell ss:StyleID="s412"><Data ss:Type="Number">33</Data></Cell>
              <Cell ss:StyleID="s412"><Data ss:Type="Number">34</Data></Cell>
              <Cell ss:StyleID="s412"><Data ss:Type="Number">35</Data></Cell>
              <Cell ss:StyleID="s412"><Data ss:Type="Number">36</Data></Cell>
              <Cell ss:StyleID="s412"/>
              <Cell ss:StyleID="s412"/>
              <Cell ss:StyleID="s412"/>
              <Cell ss:StyleID="s412"><Data ss:Type="Number">37</Data></Cell>
              <Cell ss:StyleID="s412"><Data ss:Type="Number">38</Data></Cell>
              <Cell ss:StyleID="s412"><Data ss:Type="Number">39</Data></Cell>
              <Cell ss:StyleID="s412"><Data ss:Type="Number">40</Data></Cell>
              <Cell ss:StyleID="s412"><Data ss:Type="Number">41</Data></Cell>
              <Cell ss:StyleID="s412"><Data ss:Type="Number">42</Data></Cell>
              <Cell ss:StyleID="s412"><Data ss:Type="Number">43</Data></Cell>
              <Cell ss:StyleID="s412"><Data ss:Type="Number">44</Data></Cell>
              <Cell ss:StyleID="s412"><Data ss:Type="Number">45</Data></Cell>
              <Cell ss:StyleID="s412"><Data ss:Type="Number">46</Data></Cell>
              <Cell ss:StyleID="s412"><Data ss:Type="Number">47</Data></Cell>
            </Row>
            <!-- <<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<< Данные таблицы >>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>> -->
            <xsl:apply-templates select="//TS1" />
          </Table>
          <WorksheetOptions xmlns="urn:schemas-microsoft-com:office:excel">
            <Zoom>80</Zoom>
          </WorksheetOptions>
        </Worksheet>
      </xsl:when>
      <xsl:when test="TS2 = 1">
        <!-- <<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<< Лист ТХ2 >>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>> -->
        <Worksheet ss:Name="ТХ2">
          <Table>
            <Column ss:AutoFitWidth="0" ss:Width="21" />
            <Column ss:AutoFitWidth="0" ss:Width="113.25" />
            <Column ss:AutoFitWidth="0" ss:Width="100.5" />
            <Column ss:AutoFitWidth="0" ss:Width="114.75" />
            <Column ss:Index="6" ss:AutoFitWidth="0" ss:Width="87.75" />
            <Column ss:Index="8" ss:AutoFitWidth="0" ss:Width="62.25" />
            <Column ss:AutoFitWidth="0" ss:Width="71.25" />
            <Column ss:AutoFitWidth="0" ss:Width="69.75" />
            <Column ss:AutoFitWidth="0" ss:Width="76.5" />
            <Column ss:AutoFitWidth="0" ss:Width="69.75" />
            <Column ss:AutoFitWidth="0" ss:Width="73.5" />
            <Column ss:AutoFitWidth="0" ss:Width="83.25" />
            <Column ss:AutoFitWidth="0" ss:Width="65.25" />
            <Column ss:AutoFitWidth="0" ss:Width="51.75" />
            <Column ss:Index="18" ss:AutoFitWidth="0" ss:Width="60.75" />
            <Column ss:Index="20" ss:AutoFitWidth="0" ss:Width="59.25" />
            <Column ss:AutoFitWidth="0" ss:Width="60" />
            <Column ss:AutoFitWidth="0" ss:Width="65.25" />
            <Column ss:AutoFitWidth="0" ss:Width="51.75" />
            <Column ss:AutoFitWidth="0" ss:Width="51" />
            <Column ss:AutoFitWidth="0" ss:Width="83.25" />
            <Row ss:Index="2" ss:AutoFitHeight="0">
              <Cell ss:Index="2" ss:MergeAcross="5" ss:StyleID="s19624"><Data ss:Type="String">Общие данные</Data></Cell>
              <Cell ss:MergeAcross="6" ss:StyleID="s19624"><Data ss:Type="String">Характеристика объекта</Data></Cell>
              <Cell ss:MergeAcross="3" ss:StyleID="m166230568"><Data ss:Type="String">Тепловая нагрузка (мощность) по договорам теплоснабжения, ккал/час</Data></Cell>
              <Cell ss:MergeAcross="7" ss:StyleID="m166230608"><Data ss:Type="String">Потребление тепла на регулируемый период</Data></Cell>
            </Row>
            <Row ss:Height="60">
              <Cell ss:Index="2" ss:StyleID="s19617"><Data ss:Type="String">Производственный филиал (район)</Data></Cell>
              <Cell ss:StyleID="s19617"><Data ss:Type="String">Населённый пункт</Data></Cell>
              <Cell ss:StyleID="s19617"><Data ss:Type="String">Идентификационный код источника теплоснабжения</Data></Cell>
              <Cell ss:StyleID="s19617"><Data ss:Type="String">Тип</Data></Cell>
              <Cell ss:StyleID="s19617"><Data ss:Type="String">Наименование объекта</Data></Cell>
              <Cell ss:StyleID="s19618"><Data ss:Type="String">Адрес</Data></Cell>
              <Cell ss:StyleID="s19617"><Data ss:Type="String">Этажность</Data></Cell>
              <Cell ss:StyleID="s19617"><Data ss:Type="String">Отапливаемая площадь, м2</Data></Cell>
              <Cell ss:StyleID="s19617"><Data ss:Type="String">Отапливаемый объём, м3</Data></Cell>
              <Cell ss:StyleID="s19617"><Data ss:Type="String">Внутренняя температура, °C</Data></Cell>
              <Cell ss:StyleID="s19617"><Data ss:Type="String">Количество работающих, чел.</Data></Cell>
              <Cell ss:StyleID="s19617"><Data ss:Type="String">Норма потребления воды, л/сут.</Data></Cell>
              <Cell ss:StyleID="s19617"><Data ss:Type="String">Удельный расход тепла на отопление, ккал/(м3∗ч∗°C)</Data></Cell>
              <Cell ss:StyleID="s19617"><Data ss:Type="String">Всего тепловая нагрузка (мощность)</Data></Cell>
              <Cell ss:StyleID="s19617"><Data ss:Type="String">на теплоснабжение</Data></Cell>
              <Cell ss:StyleID="s19617"><Data ss:Type="String">из центр. системы ГВС</Data></Cell>
              <Cell ss:StyleID="s19617"><Data ss:Type="String">на ГВС из системы отопления</Data></Cell>
              <Cell ss:StyleID="s19617"><Data ss:Type="String">Всего, Гкал</Data></Cell>
              <Cell ss:StyleID="s19619"><Data ss:Type="String">Отопление</Data></Cell>
              <Cell ss:StyleID="s19620"><Data ss:Type="String">ГВС из системы отопления</Data></Cell>
              <Cell ss:StyleID="s19620"><Data ss:Type="String">централизованная ГВС</Data></Cell>
              <Cell ss:StyleID="s19617"><Data ss:Type="String">Спутники</Data></Cell>
              <Cell ss:StyleID="s19617"><Data ss:Type="String">Теплообменники</Data></Cell>
              <Cell ss:StyleID="s19621"><Data ss:Type="String">Кол-во дней подключенных к системе</Data></Cell>
              <Cell ss:StyleID="s19686"><Data ss:Type="String">Примечание</Data></Cell>
            </Row>
            <!-- <<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<< Данные таблицы >>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>> -->
            <xsl:apply-templates select="//TS2" />
          </Table>
          <WorksheetOptions xmlns="urn:schemas-microsoft-com:office:excel">
            <Zoom>80</Zoom>
          </WorksheetOptions>
        </Worksheet>
      </xsl:when>
      <xsl:when test="TS3 = 1">
        <!-- <<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<< Лист ТХ3 >>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>> -->
        <Worksheet ss:Name="ТХ3">
          <Table>
            <Column ss:AutoFitWidth="0" ss:Width="46.5"/>
            <Column ss:AutoFitWidth="0" ss:Width="1000"/>
            <Column ss:AutoFitWidth="0" ss:Width="127.5"/>
            <Column ss:AutoFitWidth="0" ss:Width="124.5"/>
            <Column ss:AutoFitWidth="0" ss:Width="76.5"/>
            <Column ss:AutoFitWidth="0" ss:Width="78"/>
            <Column ss:AutoFitWidth="0" ss:Width="60"/>
            <Column ss:AutoFitWidth="0" ss:Width="48.75" ss:Span="2"/>
            <Column ss:Index="11" ss:AutoFitWidth="1" ss:Width="54"/>
            <Column ss:AutoFitWidth="1" ss:Width="48.75" ss:Span="45"/>
            <Column ss:Index="58" ss:AutoFitWidth="0" ss:Width="76.5"/>
            <Row ss:Height="12.75">
              <Cell ss:Index="2" ss:StyleID="s395"><Data ss:Type="String">Потребление тепловой энергии</Data></Cell>
            </Row>
            <Row ss:Height="12.75">
              <Cell ss:Index="2" ss:StyleID="s402"><ss:Data ss:Type="String" xmlns="http://www.w3.org/TR/REC-html40"><B>по предприятию <U>___</U></B><I><U><Font html:Color="#FF0000">(указать наименование организации)</Font></U></I><B><I><U>_</U></I><U>_</U></B></ss:Data></Cell>
            </Row>
            <Row ss:Height="12.75">
              <Cell ss:Index="2" ss:StyleID="s402"><ss:Data ss:Type="String" xmlns="http://www.w3.org/TR/REC-html40"><B>на _</B><I><U><Font html:Color="#FF0000">(указать регулируемый период)</Font></U></I><B>_ год</B></ss:Data></Cell>
            </Row>
            <Row ss:Height="12.75"/>
            <Row ss:Height="12.75">
              <Cell ss:MergeDown="5" ss:StyleID="s459"><Data ss:Type="String">№ п/п</Data></Cell>
              <Cell ss:MergeDown="5" ss:StyleID="s490"><Data ss:Type="String">Категория потребителя</Data></Cell>
              <Cell ss:MergeDown="5" ss:StyleID="m201520164"><Data ss:Type="String">Район</Data></Cell>
              <Cell ss:MergeDown="5" ss:StyleID="m201520184"><Data ss:Type="String">Наслег</Data></Cell>
              <Cell ss:MergeDown="5" ss:StyleID="s459"><Data ss:Type="String">населенного пункта</Data></Cell>
              <Cell ss:MergeDown="5" ss:StyleID="s459"><Data ss:Type="String">Наименование котельной</Data></Cell>
              <Cell ss:MergeDown="5" ss:StyleID="s490"><Data ss:Type="String">Идентиф. код  котельной</Data></Cell>
              <Cell ss:MergeAcross="1" ss:MergeDown="3" ss:StyleID="s459"><Data ss:Type="String">Вновь вводимые объекты</Data></Cell>
              <Cell ss:MergeDown="5" ss:StyleID="s479"><Data ss:Type="String">Степень благоустройства</Data></Cell>
              <Cell ss:MergeDown="5" ss:StyleID="s480"><Data ss:Type="String">Продолж. отоп. периода, сут.</Data></Cell>
              <Cell ss:MergeAcross="3" ss:MergeDown="1" ss:StyleID="s469"><ss:Data ss:Type="String" xmlns="http://www.w3.org/TR/REC-html40">Тепловая нагрузка (мощность) по договорам теплоснабжения, Гкал/час</ss:Data></Cell>
              <Cell ss:MergeAcross="13" ss:MergeDown="1" ss:StyleID="s482"><Data ss:Type="String">Утвержденные в тарифе объемы тепла в отчетном периоде</Data></Cell>
              <Cell ss:MergeAcross="13" ss:MergeDown="1" ss:StyleID="s483"><Data ss:Type="String">Прогноз потребления тепла на регулируемый период</Data></Cell>
              <Cell ss:MergeAcross="13" ss:MergeDown="1" ss:StyleID="s478"><Data ss:Type="String">Отклонение прогноза от тарифа</Data></Cell>
              <Cell ss:MergeDown="5" ss:StyleID="s467"><Data ss:Type="String">Причины изменения</Data></Cell>
            </Row>
            <Row ss:Height="12.75"/>
            <Row ss:Height="12.75">
              <Cell ss:Index="12" ss:MergeDown="3" ss:StyleID="s469"><Data ss:Type="String">Всего, Гкал/час</Data></Cell>
              <Cell ss:MergeAcross="2" ss:StyleID="s460"><Data ss:Type="String">в том числе</Data></Cell>
              <Cell ss:MergeDown="3" ss:StyleID="s470"><Data ss:Type="String">Этажность</Data></Cell>
              <Cell ss:MergeDown="3" ss:StyleID="m201519504"><Data ss:Type="String">Степень благоустройства</Data></Cell>
              <Cell ss:MergeDown="3" ss:StyleID="m201519524"><Data ss:Type="String">Количество потребителей, чел</Data></Cell>
              <Cell ss:MergeDown="3" ss:StyleID="m201519544"><Data ss:Type="String">Остекление окон (двойное-2/тройное-3)</Data></Cell>
              <Cell ss:MergeAcross="1" ss:MergeDown="2" ss:StyleID="s475"><Data ss:Type="String">Отапливаемая (ый) площадь (объем)</Data></Cell>
              <Cell ss:MergeDown="3" ss:StyleID="s475"><Data ss:Type="String">Всего, Гкал</Data></Cell>
              <Cell ss:MergeAcross="6" ss:StyleID="s477"><Data ss:Type="String">в том числе</Data></Cell>
              <Cell ss:MergeDown="3" ss:StyleID="s455"><Data ss:Type="String">Этажность</Data></Cell>
              <Cell ss:MergeDown="3" ss:StyleID="m201519784"><Data ss:Type="String">Степень благоустройства</Data></Cell>
              <Cell ss:MergeDown="3" ss:StyleID="m201519804"><Data ss:Type="String">Количество потребителей, чел</Data></Cell>
              <Cell ss:MergeDown="3" ss:StyleID="m201519824"><Data ss:Type="String">Остекление окон (двойное-2/тройное-3)</Data></Cell>
              <Cell ss:MergeAcross="1" ss:MergeDown="2" ss:StyleID="s455"><Data ss:Type="String">Отапливаемая (ый) площадь (объем)</Data></Cell>
              <Cell ss:MergeDown="3" ss:StyleID="s455"><Data ss:Type="String">Всего, Гкал</Data></Cell>
              <Cell ss:MergeAcross="6" ss:StyleID="s457"><Data ss:Type="String">в том числе</Data></Cell>
              <Cell ss:MergeDown="3" ss:StyleID="s411"><Data ss:Type="String">Этажность</Data></Cell>
              <Cell ss:MergeDown="3" ss:StyleID="m201519664"><Data ss:Type="String">Степень благоустройства</Data></Cell>
              <Cell ss:MergeDown="3" ss:StyleID="m201519684"><Data ss:Type="String">Количество потребителей, чел</Data></Cell>
              <Cell ss:MergeDown="3" ss:StyleID="m143752700"><Data ss:Type="String">Остекление окон (двойное-2/тройное-3)</Data></Cell>
              <Cell ss:MergeAcross="1" ss:MergeDown="2" ss:StyleID="s411"><Data ss:Type="String">Отапливаемая (ый) площадь (объем)</Data></Cell>
              <Cell ss:MergeDown="3" ss:StyleID="s411"><Data ss:Type="String">Всего, Гкал</Data></Cell>
              <Cell ss:MergeAcross="6" ss:StyleID="s466"><Data ss:Type="String">в том числе</Data></Cell>
            </Row>
            <Row ss:Height="12.75">
              <Cell ss:Index="13" ss:MergeDown="2" ss:StyleID="s460"><Data ss:Type="String">на теплоснабжение</Data></Cell>
              <Cell ss:MergeAcross="1" ss:StyleID="s487"><Data ss:Type="String">на горячее водоснабжение</Data></Cell>
              <Cell ss:Index="23" ss:MergeAcross="3" ss:StyleID="s475"><Data ss:Type="String">на теплоснабжение</Data></Cell>
              <Cell ss:MergeAcross="2" ss:StyleID="s488"><Data ss:Type="String">на горячее водоснабжение</Data></Cell>
              <Cell ss:Index="37" ss:MergeAcross="3" ss:StyleID="s455"><Data ss:Type="String">на теплоснабжение</Data></Cell>
              <Cell ss:MergeAcross="2" ss:StyleID="s489"><Data ss:Type="String">на горячее водоснабжение</Data></Cell>
              <Cell ss:Index="51" ss:MergeAcross="3" ss:StyleID="s411"><Data ss:Type="String">на теплоснабжение</Data></Cell>
              <Cell ss:MergeAcross="2" ss:StyleID="s458"><Data ss:Type="String">на горячее водоснабжение</Data></Cell>
            </Row>
            <Row ss:Height="12.75">
              <Cell ss:Index="8" ss:MergeDown="1" ss:StyleID="s459"><Data ss:Type="String">кол-во, шт</Data></Cell>
              <Cell ss:MergeDown="1" ss:StyleID="s459"><Data ss:Type="String">дата ввода</Data></Cell>
              <Cell ss:Index="14" ss:MergeDown="1" ss:StyleID="s460"><Data ss:Type="String">из центр. системы ГВС</Data></Cell>
              <Cell ss:MergeDown="1" ss:StyleID="s460"><Data ss:Type="String">из системы отопления</Data></Cell>
              <Cell ss:Index="23" ss:MergeDown="1" ss:StyleID="s409"><Data ss:Type="String">на отопление</Data></Cell>
              <Cell ss:MergeDown="1" ss:StyleID="s409"><Data ss:Type="String">на вентиляцию</Data></Cell>
              <Cell ss:MergeDown="1" ss:StyleID="s409"><Data ss:Type="String">спутники ТС</Data></Cell>
              <Cell ss:MergeDown="1" ss:StyleID="s409"><Data ss:Type="String">потери в абон.сетях ТС</Data></Cell>
              <Cell ss:MergeDown="1" ss:StyleID="s409"><Data ss:Type="String"> из центр. системы ГВС</Data></Cell>
              <Cell ss:MergeDown="1" ss:StyleID="s409"><Data ss:Type="String">из системы отопления</Data></Cell>
              <Cell ss:MergeDown="1" ss:StyleID="s409"><Data ss:Type="String">теплообменники</Data></Cell>
              <Cell ss:Index="37" ss:MergeDown="1" ss:StyleID="s410"><Data ss:Type="String">на отопление</Data></Cell>
              <Cell ss:MergeDown="1" ss:StyleID="s410"><Data ss:Type="String">на вентиляцию</Data></Cell>
              <Cell ss:MergeDown="1" ss:StyleID="s410"><Data ss:Type="String">спутники ТС</Data></Cell>
              <Cell ss:MergeDown="1" ss:StyleID="s410"><Data ss:Type="String">потери в сетях ТС</Data></Cell>
              <Cell ss:MergeDown="1" ss:StyleID="s410"><Data ss:Type="String"> из центр. системы ГВС</Data></Cell>
              <Cell ss:MergeDown="1" ss:StyleID="s410"><Data ss:Type="String">из системы отопления</Data></Cell>
              <Cell ss:MergeDown="1" ss:StyleID="s410"><Data ss:Type="String">теплообменники</Data></Cell>
              <Cell ss:Index="51" ss:MergeDown="1" ss:StyleID="s411"><Data ss:Type="String">на отопление</Data></Cell>
              <Cell ss:MergeDown="1" ss:StyleID="s411"><Data ss:Type="String">на вентиляцию</Data></Cell>
              <Cell ss:MergeDown="1" ss:StyleID="s411"><Data ss:Type="String">спутники ТС</Data></Cell>
              <Cell ss:MergeDown="1" ss:StyleID="s411"><Data ss:Type="String">потери в сетях ТС</Data></Cell>
              <Cell ss:MergeDown="1" ss:StyleID="s411"><Data ss:Type="String"> из центр. системы ГВС</Data></Cell>
              <Cell ss:MergeDown="1" ss:StyleID="s411"><Data ss:Type="String">из системы отопления</Data></Cell>
              <Cell ss:MergeDown="1" ss:StyleID="s411"><Data ss:Type="String">теплообменники</Data></Cell>
            </Row>
            <Row ss:Height="28.5">
              <Cell ss:Index="20" ss:StyleID="s409"><Data ss:Type="String">м2</Data></Cell>
              <Cell ss:StyleID="s409"><Data ss:Type="String">м3</Data></Cell>
              <Cell ss:Index="34" ss:StyleID="s410"><Data ss:Type="String">м2</Data></Cell>
              <Cell ss:StyleID="s410"><Data ss:Type="String">м3</Data></Cell>
              <Cell ss:Index="48" ss:StyleID="s411"><Data ss:Type="String">м2</Data></Cell>
              <Cell ss:StyleID="s411"><Data ss:Type="String">м3</Data></Cell>
            </Row>
            <Row ss:Height="12.75">
              <Cell ss:StyleID="s412"><Data ss:Type="Number">1</Data></Cell>
              <Cell ss:StyleID="s412"><Data ss:Type="Number">2</Data></Cell>
              <Cell ss:StyleID="s412"/>
              <Cell ss:StyleID="s412"/>
              <Cell ss:StyleID="s412"><Data ss:Type="Number">3</Data></Cell>
              <Cell ss:StyleID="s412"><Data ss:Type="Number">4</Data></Cell>
              <Cell ss:StyleID="s412"><Data ss:Type="Number">5</Data></Cell>
              <Cell ss:StyleID="s412"><Data ss:Type="Number">6</Data></Cell>
              <Cell ss:StyleID="s412"><Data ss:Type="Number">7</Data></Cell>
              <Cell ss:StyleID="s412"><Data ss:Type="Number">8</Data></Cell>
              <Cell ss:StyleID="s412"><Data ss:Type="Number">9</Data></Cell>
              <Cell ss:StyleID="s412"><Data ss:Type="Number">10</Data></Cell>
              <Cell ss:StyleID="s412"><Data ss:Type="Number">11</Data></Cell>
              <Cell ss:StyleID="s412"><Data ss:Type="Number">12</Data></Cell>
              <Cell ss:StyleID="s412"><Data ss:Type="Number">13</Data></Cell>
              <Cell ss:StyleID="s412"><Data ss:Type="Number">14</Data></Cell>
              <Cell ss:StyleID="s412"/>
              <Cell ss:StyleID="s412"/>
              <Cell ss:StyleID="s412"/>
              <Cell ss:StyleID="s412"><Data ss:Type="Number">15</Data></Cell>
              <Cell ss:StyleID="s412"><Data ss:Type="Number">16</Data></Cell>
              <Cell ss:StyleID="s412"><Data ss:Type="Number">17</Data></Cell>
              <Cell ss:StyleID="s412"><Data ss:Type="Number">18</Data></Cell>
              <Cell ss:StyleID="s412"><Data ss:Type="Number">19</Data></Cell>
              <Cell ss:StyleID="s412"><Data ss:Type="Number">20</Data></Cell>
              <Cell ss:StyleID="s412"><Data ss:Type="Number">21</Data></Cell>
              <Cell ss:StyleID="s412"><Data ss:Type="Number">22</Data></Cell>
              <Cell ss:StyleID="s412"><Data ss:Type="Number">23</Data></Cell>
              <Cell ss:StyleID="s412"><Data ss:Type="Number">24</Data></Cell>
              <Cell ss:StyleID="s412"><Data ss:Type="Number">25</Data></Cell>
              <Cell ss:StyleID="s412"/>
              <Cell ss:StyleID="s412"/>
              <Cell ss:StyleID="s412"/>
              <Cell ss:StyleID="s412"><Data ss:Type="Number">26</Data></Cell>
              <Cell ss:StyleID="s412"><Data ss:Type="Number">27</Data></Cell>
              <Cell ss:StyleID="s412"><Data ss:Type="Number">28</Data></Cell>
              <Cell ss:StyleID="s412"><Data ss:Type="Number">29</Data></Cell>
              <Cell ss:StyleID="s412"><Data ss:Type="Number">30</Data></Cell>
              <Cell ss:StyleID="s412"><Data ss:Type="Number">31</Data></Cell>
              <Cell ss:StyleID="s412"><Data ss:Type="Number">32</Data></Cell>
              <Cell ss:StyleID="s412"><Data ss:Type="Number">33</Data></Cell>
              <Cell ss:StyleID="s412"><Data ss:Type="Number">34</Data></Cell>
              <Cell ss:StyleID="s412"><Data ss:Type="Number">35</Data></Cell>
              <Cell ss:StyleID="s412"><Data ss:Type="Number">36</Data></Cell>
              <Cell ss:StyleID="s412"/>
              <Cell ss:StyleID="s412"/>
              <Cell ss:StyleID="s412"/>
              <Cell ss:StyleID="s412"><Data ss:Type="Number">37</Data></Cell>
              <Cell ss:StyleID="s412"><Data ss:Type="Number">38</Data></Cell>
              <Cell ss:StyleID="s412"><Data ss:Type="Number">39</Data></Cell>
              <Cell ss:StyleID="s412"><Data ss:Type="Number">40</Data></Cell>
              <Cell ss:StyleID="s412"><Data ss:Type="Number">41</Data></Cell>
              <Cell ss:StyleID="s412"><Data ss:Type="Number">42</Data></Cell>
              <Cell ss:StyleID="s412"><Data ss:Type="Number">43</Data></Cell>
              <Cell ss:StyleID="s412"><Data ss:Type="Number">44</Data></Cell>
              <Cell ss:StyleID="s412"><Data ss:Type="Number">45</Data></Cell>
              <Cell ss:StyleID="s412"><Data ss:Type="Number">46</Data></Cell>
              <Cell ss:StyleID="s412"><Data ss:Type="Number">47</Data></Cell>
            </Row>
            <!-- <<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<< Данные таблицы >>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>> -->
            <xsl:apply-templates select="//TS3" />
          </Table>
          <WorksheetOptions xmlns="urn:schemas-microsoft-com:office:excel">
            <Zoom>80</Zoom>
          </WorksheetOptions>
        </Worksheet>
      </xsl:when>
      <xsl:when test="TS5 = 1">
        <!-- <<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<< Лист ТХ5 >>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>> -->
        <Worksheet ss:Name="ТХ5">
          <Table>
            <Column ss:AutoFitWidth="0" ss:Width="52.5"/>
            <Column ss:AutoFitWidth="0" ss:Width="410.25"/>
            <Column ss:AutoFitWidth="0" ss:Width="70.5"/>
            <!-- Надо бы тут заюзать функцию для определения количества столбцов или же вычитать с отдельного поля в DataSet количество столбцов -->
            <xsl:call-template name="TS5PrintColumns"><xsl:with-param name="pStart" select="1"/><xsl:with-param name="pEnd" select="500"/><xsl:with-param name="pWidth" select="87.75"/></xsl:call-template>
            <Row>
              <Cell ss:StyleID="s40077"/>
              <Cell ss:StyleID="s39922"><Data ss:Type="String">ПП-5. Индикаторы ГУП &quot;ЖКХ РС(Я)&quot; по производству тепловой энергии</Data></Cell>
              <Cell ss:StyleID="s39928"/>
            </Row>
            <Row>
              <Cell ss:StyleID="s40077"/>
              <Cell ss:Index="3" ss:StyleID="s39928"/>
            </Row>
            <!-- <<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<< Данные таблицы >>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>> -->
            <xsl:apply-templates select="//TS5" />
          </Table>
          <WorksheetOptions xmlns="urn:schemas-microsoft-com:office:excel">
            <Zoom>60</Zoom>
          </WorksheetOptions>
        </Worksheet>
      </xsl:when>
      <xsl:when test="TS6 = 1">
        <!-- <<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<< Лист ТХ6 >>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>> -->
        <Worksheet ss:Name="ТХ6">
          <Table>
            <Column ss:Width="91.5" />
            <Column ss:Width="54.75" />
            <Column ss:Width="66" />
            <Column ss:Width="57" />
            <Column ss:Width="72" />
            <Column ss:Width="50.25" />
            <Column ss:Width="56.25" />
            <Column ss:Width="46.5" />
            <Column ss:Width="58.5" />
            <Column ss:Width="55" />
            <Column ss:Width="50.25" />
            <Column ss:Width="47.25" />
            <Column ss:Width="43.5" />
            <Column ss:Width="46.5" />
            <Column ss:Width="47.25" />
            <!-- <<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<< Заголовок >>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>> -->
            <Row>
              <Cell ss:Index="15" ss:StyleID="TS6Title"><Data ss:Type="String">Форма 6 (ТХ)</Data></Cell>
            </Row>
            <Row ss:Index="2">
              <Cell ss:Index="6" ss:StyleID="TS6Title"><Data ss:Type="String">Техническая характеристика котельных</Data></Cell>
            </Row>
            <Row>
              <Cell ss:Index="6" ss:StyleID="TS6Title"><Data ss:Type="String">по филиалу-участку</Data></Cell>
            </Row>
            <Row>
              <Cell ss:Index="6" ss:StyleID="TS6Title"><Data ss:Type="String">на год</Data></Cell>
            </Row>
            <!-- <<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<< Столбцы таблицы >>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>> -->
            <Row ss:Height="50" ss:Index="6">
              <Cell ss:MergeDown="1" ss:StyleID="TS6Column"><Data ss:Type="String">Марки котлов</Data></Cell>
              <Cell ss:MergeDown="1" ss:StyleID="TS6Column"><Data ss:Type="String">Теплоноси-тель</Data></Cell>
              <Cell ss:MergeDown="1" ss:StyleID="TS6Column"><Data ss:Type="String">Назначение котлов</Data></Cell>
              <Cell ss:MergeDown="1" ss:StyleID="TS6Column"><Data ss:Type="String">Вид топлива</Data></Cell>
              <Cell ss:MergeDown="1" ss:StyleID="TS6Column"><Data ss:Type="String">Топливоподача</Data></Cell>
              <Cell ss:MergeDown="1" ss:StyleID="TS6Column"><Data ss:Type="String">Наличие оборудования ХВО</Data></Cell>
              <Cell ss:MergeDown="1" ss:StyleID="TS6Column"><Data ss:Type="String">Уст. мощность, Гкал/час</Data></Cell>
              <Cell ss:MergeDown="1" ss:StyleID="TS6Column"><Data ss:Type="String">КПД котлов, коэф-фициент</Data></Cell>
              <Cell ss:MergeDown="1" ss:StyleID="TS6Column"><Data ss:Type="String">Объем выраба-тываемой тепловой энергии, Гкал</Data></Cell>
              <Cell ss:MergeAcross="1" ss:StyleID="TS6Column"><Data ss:Type="String">Нагрузка</Data></Cell>
              <Cell ss:MergeAcross="1" ss:StyleID="TS6Column"><Data ss:Type="String">Коэфф. использования мощности</Data></Cell>
              <Cell ss:MergeDown="1" ss:StyleID="TS6Column"><Data ss:Type="String">Год ввода</Data></Cell>
              <Cell ss:MergeDown="1" ss:StyleID="TS6Column"><Data ss:Type="String">Год послед-него кап. ремонта</Data></Cell>
            </Row>
            <Row ss:Height="50">
              <Cell ss:Index="10" ss:StyleID="TS6Column"><Data ss:Type="String">Среднеото-пительная, Гкал/час</Data></Cell>
              <Cell ss:StyleID="TS6Column"><Data ss:Type="String">Пиковая, Гкал/час</Data></Cell>
              <Cell ss:StyleID="TS6Column"><Data ss:Type="String">За отопит. период</Data></Cell>
              <Cell ss:StyleID="TS6Column"><Data ss:Type="String">В пиков. нагрузки</Data></Cell>
            </Row>
            <Row>
              <Cell ss:StyleID="TS6Column"><Data ss:Type="Number">1</Data></Cell>
              <Cell ss:StyleID="TS6Column"><Data ss:Type="Number">2</Data></Cell>
              <Cell ss:StyleID="TS6Column"><Data ss:Type="Number">3</Data></Cell>
              <Cell ss:StyleID="TS6Column"><Data ss:Type="Number">4</Data></Cell>
              <Cell ss:StyleID="TS6Column"><Data ss:Type="Number">5</Data></Cell>
              <Cell ss:StyleID="TS6Column"><Data ss:Type="Number">6</Data></Cell>
              <Cell ss:StyleID="TS6Column"><Data ss:Type="Number">7</Data></Cell>
              <Cell ss:StyleID="TS6Column"><Data ss:Type="Number">8</Data></Cell>
              <Cell ss:StyleID="TS6Column"><Data ss:Type="Number">9</Data></Cell>
              <Cell ss:StyleID="TS6Column"><Data ss:Type="Number">10</Data></Cell>
              <Cell ss:StyleID="TS6Column"><Data ss:Type="Number">11</Data></Cell>
              <Cell ss:StyleID="TS6Column"><Data ss:Type="Number">12</Data></Cell>
              <Cell ss:StyleID="TS6Column"><Data ss:Type="Number">13</Data></Cell>
              <Cell ss:StyleID="TS6Column"><Data ss:Type="Number">14</Data></Cell>
              <Cell ss:StyleID="TS6Column"><Data ss:Type="Number">15</Data></Cell>
            </Row>
            <!-- <<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<< Данные таблицы >>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>> -->
            <xsl:apply-templates select="//TS6" />
          </Table>
        </Worksheet>
      </xsl:when>
      <xsl:when test="TS7 = 1">
        <!-- <<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<< Лист ТХ7 >>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>> -->
        <Worksheet ss:Name="ТХ7">
          <Table ss:DefaultColumnWidth="91.5">
            <Column ss:Index="13" ss:Width="97.5" />
            <!-- <<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<< Заголовок >>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>> -->
            <Row>
              <Cell ss:Index="15" ss:StyleID="TS7Title"><Data ss:Type="String">Форма 7 (ТХ)</Data></Cell>
            </Row>
            <Row ss:Index="2">
              <Cell ss:Index="8" ss:StyleID="TS7Title"><Data ss:Type="String">Техническая характеристика</Data></Cell>
            </Row>
            <Row>
              <Cell ss:Index="8" ss:StyleID="TS7Title"><Data ss:Type="String">электросилового оборудования теплоэнергетического хозяйства</Data></Cell>
            </Row>
            <Row>
              <Cell ss:Index="8" ss:StyleID="TS7Title"><Data ss:Type="String">по участку</Data></Cell>
            </Row>
            <Row>
              <Cell ss:Index="8" ss:StyleID="TS7Title"><Data ss:Type="String">на год</Data></Cell>
            </Row>
            <!-- <<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<< Столбцы таблицы >>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>> -->
            <Row ss:Height="78" ss:Index="7">
              <Cell ss:MergeDown="1" ss:StyleID="TS7Column"><Data ss:Type="String">Филиал</Data></Cell>
              <Cell ss:MergeDown="1" ss:StyleID="TS7Column"><Data ss:Type="String">Населенный пункт</Data></Cell>
              <Cell ss:MergeDown="1" ss:StyleID="TS7Column"><Data ss:Type="String">Наименование котельной</Data></Cell>
              <Cell ss:MergeDown="1" ss:StyleID="TS7Column"><Data ss:Type="String">Наименование оборудования</Data></Cell>
              <Cell ss:MergeDown="1" ss:StyleID="TS7Column"><Data ss:Type="String">Марка оборудования</Data></Cell>
              <Cell ss:MergeDown="1" ss:StyleID="TS7Column"><Data ss:Type="String">Марка электросилового агрегата</Data></Cell>
              <Cell ss:MergeDown="1" ss:StyleID="TS7Column"><Data ss:Type="String">Год установки</Data></Cell>
              <Cell ss:MergeDown="1" ss:StyleID="TS7Column"><Data ss:Type="String">Кол-во однотипных оборудований, шт.</Data></Cell>
              <Cell ss:MergeDown="1" ss:StyleID="TS7Column"><Data ss:Type="String">Мощность электросилового агрегата, кВт*ч</Data></Cell>
              <Cell ss:MergeDown="1" ss:StyleID="TS7Column"><Data ss:Type="String">Суммарн. мощность электросиловых агрегатов, кВт*ч</Data></Cell>
              <Cell ss:MergeDown="1" ss:StyleID="TS7Column"><Data ss:Type="String">Режим работы (часов работы в сутки)</Data></Cell>
              <Cell ss:MergeDown="1" ss:StyleID="TS7Column"><Data ss:Type="String">Часов работы в год, час</Data></Cell>
              <Cell ss:MergeDown="1" ss:StyleID="TS7Column"><Data ss:Type="String">Коэфф. использ. мощности, %</Data></Cell>
              <Cell ss:MergeDown="1" ss:StyleID="TS7Column"><Data ss:Type="String">Потребл. электроэн. в год, кВт*ч</Data></Cell>
              <Cell ss:MergeDown="1" ss:StyleID="TS7Column"><Data ss:Type="String">Фактический расход электроэн., кВт*ч</Data></Cell>
            </Row>
            <Row ss:Index="9">
              <Cell ss:StyleID="TS7Column"><Data ss:Type="Number">1</Data></Cell>
              <Cell ss:StyleID="TS7Column"><Data ss:Type="Number">2</Data></Cell>
              <Cell ss:StyleID="TS7Column"><Data ss:Type="Number">3</Data></Cell>
              <Cell ss:StyleID="TS7Column"><Data ss:Type="Number">4</Data></Cell>
              <Cell ss:StyleID="TS7Column"><Data ss:Type="Number">5</Data></Cell>
              <Cell ss:StyleID="TS7Column"><Data ss:Type="Number">6</Data></Cell>
              <Cell ss:StyleID="TS7Column"><Data ss:Type="Number">7</Data></Cell>
              <Cell ss:StyleID="TS7Column"><Data ss:Type="Number">8</Data></Cell>
              <Cell ss:StyleID="TS7Column"><Data ss:Type="Number">9</Data></Cell>
              <Cell ss:StyleID="TS7Column"><Data ss:Type="Number">10</Data></Cell>
              <Cell ss:StyleID="TS7Column"><Data ss:Type="Number">11</Data></Cell>
              <Cell ss:StyleID="TS7Column"><Data ss:Type="Number">12</Data></Cell>
              <Cell ss:StyleID="TS7Column"><Data ss:Type="Number">13</Data></Cell>
              <Cell ss:StyleID="TS7Column"><Data ss:Type="Number">14</Data></Cell>
              <Cell ss:StyleID="TS7Column"><Data ss:Type="Number">15</Data></Cell>
            </Row>
            <!-- <<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<< Данные таблицы >>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>> -->
            <xsl:apply-templates select="//TS7" />
          </Table>
        </Worksheet>
      </xsl:when>
      <xsl:when test="TS81 = 1">
        <!-- <<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<< Лист ТХ8 >>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>> -->
        <Worksheet ss:Name="ТХ8 (тепл.)">
          <Table>
            <Column ss:Width="83.25" />
            <Column ss:Width="87.75" />
            <Column ss:Width="52.5" />
            <Column ss:Width="85.25" />
            <Column ss:Width="52.5" />
            <Column ss:Index="7" ss:Width="51.75" />
            <Column ss:Width="60" />
            <Column ss:Index="10" ss:Width="101.25" />
            <Column ss:Width="101.25" />
            <!-- <<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<< Заголовок >>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>> -->
            <Row>
              <Cell ss:Index="5" ss:StyleID="TS8Title"><Data ss:Type="String">Расчет потерь тепла</Data></Cell>
            </Row>
            <Row>
              <Cell ss:Index="5" ss:StyleID="TS8Title"><Data ss:Type="String">в тепловых сетях теплоэнергетического хозяйства</Data></Cell>
            </Row>
            <Row>
              <Cell ss:Index="5" ss:StyleID="TS8Title"><Data ss:Type="String">по филиалу-участку</Data></Cell>
            </Row>
            <Row>
              <Cell ss:Index="5" ss:StyleID="TS8Title"><Data ss:Type="String">на год</Data></Cell>
            </Row>
            <!-- <<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<< Столбцы таблицы >>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>> -->
            <Row ss:Height="33" ss:Index="6">
              <Cell ss:MergeDown="1" ss:StyleID="TS8Column"><Data ss:Type="String">Производственные подразделения тепло-энергетического хоз-ва</Data></Cell>
              <Cell ss:MergeDown="1" ss:StyleID="TS8Column"><Data ss:Type="String">Участки тепловых сетей (адресная принадлежность)</Data></Cell>
              <Cell ss:MergeDown="1" ss:StyleID="TS8Column"><Data ss:Type="String">Год ввода в эксплуат.</Data></Cell>
              <Cell ss:MergeDown="1" ss:StyleID="TS8Column"><Data ss:Type="String">Трубопровод по назначен. (отопл., горяч. водо-снабжение.)</Data></Cell>
              <Cell ss:MergeDown="1" ss:StyleID="TS8Column"><Data ss:Type="String">Наличие водозабора из систем отопления</Data></Cell>
              <Cell ss:MergeDown="1" ss:StyleID="TS8Column"><Data ss:Type="String">Трубо-провод по исполне-нию (кол-во труб)</Data></Cell>
              <Cell ss:MergeAcross="1" ss:StyleID="TS8Column"><Data ss:Type="String">Трубопроводы</Data></Cell>
              <Cell ss:MergeDown="1" ss:StyleID="TS8Column"><Data ss:Type="String">Кол-во задвижек, шт.</Data></Cell>
              <Cell ss:MergeDown="1" ss:StyleID="TS8Column"><Data ss:Type="String">Способ прокладки (подземн., надземн.)</Data></Cell>
              <Cell ss:MergeDown="1" ss:StyleID="TS8Column"><Data ss:Type="String">Изоляция</Data></Cell>
            </Row>
            <Row ss:Height="52.5">
              <Cell ss:Index="7" ss:StyleID="TS8Column"><Data ss:Type="String">Условный наружный диаметр, мм</Data></Cell>
              <Cell ss:StyleID="TS8Column"><Data ss:Type="String">Протяжен-ность, м</Data></Cell>
            </Row>
            <Row>
              <Cell ss:MergeAcross="10" ss:StyleID="TS8Total"><Data ss:Type="String">Всего:</Data></Cell>
            </Row>
            <!-- <<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<< Данные таблицы >>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>> -->
            <xsl:apply-templates select="//TS81" />
          </Table>
        </Worksheet>
      </xsl:when>
      <xsl:when test="TS82 = 1">
        <!-- <<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<< Лист ТХ8 >>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>> -->
        <Worksheet ss:Name="ТХ8 (ГВС)">
          <Table>
            <Column ss:Width="83.25" />
            <Column ss:Width="87.75" />
            <Column ss:Width="52.5" />
            <Column ss:Width="85.25" />
            <Column ss:Width="52.5" />
            <Column ss:Index="7" ss:Width="51.75" />
            <Column ss:Width="60" />
            <Column ss:Index="10" ss:Width="101.25" />
            <Column ss:Width="101.25" />
            <!-- <<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<< Заголовок >>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>> -->
            <Row>
              <Cell ss:Index="5" ss:StyleID="TS8Title"><Data ss:Type="String">Расчет потерь тепла</Data></Cell>
            </Row>
            <Row>
              <Cell ss:Index="5" ss:StyleID="TS8Title"><Data ss:Type="String">в тепловых сетях теплоэнергетического хозяйства</Data></Cell>
            </Row>
            <Row>
              <Cell ss:Index="5" ss:StyleID="TS8Title"><Data ss:Type="String">по филиалу-участку</Data></Cell>
            </Row>
            <Row>
              <Cell ss:Index="5" ss:StyleID="TS8Title"><Data ss:Type="String">на год</Data></Cell>
            </Row>
            <!-- <<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<< Столбцы таблицы >>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>> -->
            <Row ss:Height="33" ss:Index="6">
              <Cell ss:MergeDown="1" ss:StyleID="TS8Column"><Data ss:Type="String">Производственные подразделения тепло-энергетического хоз-ва</Data></Cell>
              <Cell ss:MergeDown="1" ss:StyleID="TS8Column"><Data ss:Type="String">Участки тепловых сетей (адресная принадлежность)</Data></Cell>
              <Cell ss:MergeDown="1" ss:StyleID="TS8Column"><Data ss:Type="String">Год ввода в эксплуат.</Data></Cell>
              <Cell ss:MergeDown="1" ss:StyleID="TS8Column"><Data ss:Type="String">Трубопровод по назначен. (отопл., горяч. водо-снабжение.)</Data></Cell>
              <Cell ss:MergeDown="1" ss:StyleID="TS8Column"><Data ss:Type="String">Наличие водозабора из систем отопления</Data></Cell>
              <Cell ss:MergeDown="1" ss:StyleID="TS8Column"><Data ss:Type="String">Трубо-провод по исполне-нию (кол-во труб)</Data></Cell>
              <Cell ss:MergeAcross="1" ss:StyleID="TS8Column"><Data ss:Type="String">Трубопроводы</Data></Cell>
              <Cell ss:MergeDown="1" ss:StyleID="TS8Column"><Data ss:Type="String">Кол-во задвижек, шт.</Data></Cell>
              <Cell ss:MergeDown="1" ss:StyleID="TS8Column"><Data ss:Type="String">Способ прокладки (подземн., надземн.)</Data></Cell>
              <Cell ss:MergeDown="1" ss:StyleID="TS8Column"><Data ss:Type="String">Изоляция</Data></Cell>
            </Row>
            <Row ss:Height="52.5">
              <Cell ss:Index="7" ss:StyleID="TS8Column"><Data ss:Type="String">Условный наружный диаметр, мм</Data></Cell>
              <Cell ss:StyleID="TS8Column"><Data ss:Type="String">Протяжен-ность, м</Data></Cell>
            </Row>
            <Row>
              <Cell ss:MergeAcross="10" ss:StyleID="TS8Total"><Data ss:Type="String">Всего:</Data></Cell>
            </Row>
            <!-- <<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<< Данные таблицы >>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>> -->
            <xsl:apply-templates select="//TS82" />
          </Table>
        </Worksheet>
      </xsl:when>
      <xsl:when test="TS83 = 1">
        <!-- <<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<< Лист ТХ8 >>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>> -->
        <Worksheet ss:Name="ТХ8 (ХВС)">
          <Table>
            <Column ss:Width="83.25" />
            <Column ss:Width="87.75" />
            <Column ss:Width="52.5" />
            <Column ss:Width="85.25" />
            <Column ss:Width="52.5" />
            <Column ss:Index="7" ss:Width="51.75" />
            <Column ss:Width="60" />
            <Column ss:Index="10" ss:Width="101.25" />
            <Column ss:Width="101.25" />
            <!-- <<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<< Заголовок >>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>> -->
            <Row>
              <Cell ss:Index="5" ss:StyleID="TS8Title"><Data ss:Type="String">Расчет потерь тепла</Data></Cell>
            </Row>
            <Row>
              <Cell ss:Index="5" ss:StyleID="TS8Title"><Data ss:Type="String">в тепловых сетях теплоэнергетического хозяйства</Data></Cell>
            </Row>
            <Row>
              <Cell ss:Index="5" ss:StyleID="TS8Title"><Data ss:Type="String">по филиалу-участку</Data></Cell>
            </Row>
            <Row>
              <Cell ss:Index="5" ss:StyleID="TS8Title"><Data ss:Type="String">на год</Data></Cell>
            </Row>
            <!-- <<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<< Столбцы таблицы >>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>> -->
            <Row ss:Height="33" ss:Index="6">
              <Cell ss:MergeDown="1" ss:StyleID="TS8Column"><Data ss:Type="String">Производственные подразделения тепло-энергетического хоз-ва</Data></Cell>
              <Cell ss:MergeDown="1" ss:StyleID="TS8Column"><Data ss:Type="String">Участки тепловых сетей (адресная принадлежность)</Data></Cell>
              <Cell ss:MergeDown="1" ss:StyleID="TS8Column"><Data ss:Type="String">Год ввода в эксплуат.</Data></Cell>
              <Cell ss:MergeDown="1" ss:StyleID="TS8Column"><Data ss:Type="String">Трубопровод по назначен. (отопл., горяч. водо-снабжение.)</Data></Cell>
              <Cell ss:MergeDown="1" ss:StyleID="TS8Column"><Data ss:Type="String">Наличие водозабора из систем отопления</Data></Cell>
              <Cell ss:MergeDown="1" ss:StyleID="TS8Column"><Data ss:Type="String">Трубо-провод по исполне-нию (кол-во труб)</Data></Cell>
              <Cell ss:MergeAcross="1" ss:StyleID="TS8Column"><Data ss:Type="String">Трубопроводы</Data></Cell>
              <Cell ss:MergeDown="1" ss:StyleID="TS8Column"><Data ss:Type="String">Кол-во задвижек, шт.</Data></Cell>
              <Cell ss:MergeDown="1" ss:StyleID="TS8Column"><Data ss:Type="String">Способ прокладки (подземн., надземн.)</Data></Cell>
              <Cell ss:MergeDown="1" ss:StyleID="TS8Column"><Data ss:Type="String">Изоляция</Data></Cell>
            </Row>
            <Row ss:Height="52.5">
              <Cell ss:Index="7" ss:StyleID="TS8Column"><Data ss:Type="String">Условный наружный диаметр, мм</Data></Cell>
              <Cell ss:StyleID="TS8Column"><Data ss:Type="String">Протяжен-ность, м</Data></Cell>
            </Row>
            <Row>
              <Cell ss:MergeAcross="10" ss:StyleID="TS8Total"><Data ss:Type="String">Всего:</Data></Cell>
            </Row>
            <!-- <<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<< Данные таблицы >>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>> -->
            <xsl:apply-templates select="//TS83" />
          </Table>
        </Worksheet>
      </xsl:when>
      <xsl:when test="TS84 = 1">
        <!-- <<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<< Лист ТХ8 >>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>> -->
        <Worksheet ss:Name="ТХ8 (кан.)">
          <Table>
            <Column ss:Width="83.25" />
            <Column ss:Width="87.75" />
            <Column ss:Width="52.5" />
            <Column ss:Width="85.25" />
            <Column ss:Width="52.5" />
            <Column ss:Index="7" ss:Width="51.75" />
            <Column ss:Width="60" />
            <Column ss:Index="10" ss:Width="101.25" />
            <Column ss:Width="101.25" />
            <!-- <<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<< Заголовок >>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>> -->
            <Row>
              <Cell ss:Index="5" ss:StyleID="TS8Title"><Data ss:Type="String">Расчет потерь тепла</Data></Cell>
            </Row>
            <Row>
              <Cell ss:Index="5" ss:StyleID="TS8Title"><Data ss:Type="String">в тепловых сетях теплоэнергетического хозяйства</Data></Cell>
            </Row>
            <Row>
              <Cell ss:Index="5" ss:StyleID="TS8Title"><Data ss:Type="String">по филиалу-участку</Data></Cell>
            </Row>
            <Row>
              <Cell ss:Index="5" ss:StyleID="TS8Title"><Data ss:Type="String">на год</Data></Cell>
            </Row>
            <!-- <<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<< Столбцы таблицы >>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>> -->
            <Row ss:Height="33" ss:Index="6">
              <Cell ss:MergeDown="1" ss:StyleID="TS8Column"><Data ss:Type="String">Производственные подразделения тепло-энергетического хоз-ва</Data></Cell>
              <Cell ss:MergeDown="1" ss:StyleID="TS8Column"><Data ss:Type="String">Участки тепловых сетей (адресная принадлежность)</Data></Cell>
              <Cell ss:MergeDown="1" ss:StyleID="TS8Column"><Data ss:Type="String">Год ввода в эксплуат.</Data></Cell>
              <Cell ss:MergeDown="1" ss:StyleID="TS8Column"><Data ss:Type="String">Трубопровод по назначен. (отопл., горяч. водо-снабжение.)</Data></Cell>
              <Cell ss:MergeDown="1" ss:StyleID="TS8Column"><Data ss:Type="String">Наличие водозабора из систем отопления</Data></Cell>
              <Cell ss:MergeDown="1" ss:StyleID="TS8Column"><Data ss:Type="String">Трубо-провод по исполне-нию (кол-во труб)</Data></Cell>
              <Cell ss:MergeAcross="1" ss:StyleID="TS8Column"><Data ss:Type="String">Трубопроводы</Data></Cell>
              <Cell ss:MergeDown="1" ss:StyleID="TS8Column"><Data ss:Type="String">Кол-во задвижек, шт.</Data></Cell>
              <Cell ss:MergeDown="1" ss:StyleID="TS8Column"><Data ss:Type="String">Способ прокладки (подземн., надземн.)</Data></Cell>
              <Cell ss:MergeDown="1" ss:StyleID="TS8Column"><Data ss:Type="String">Изоляция</Data></Cell>
            </Row>
            <Row ss:Height="52.5">
              <Cell ss:Index="7" ss:StyleID="TS8Column"><Data ss:Type="String">Условный наружный диаметр, мм</Data></Cell>
              <Cell ss:StyleID="TS8Column"><Data ss:Type="String">Протяжен-ность, м</Data></Cell>
            </Row>
            <Row>
              <Cell ss:MergeAcross="10" ss:StyleID="TS8Total"><Data ss:Type="String">Всего:</Data></Cell>
            </Row>
            <!-- <<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<< Данные таблицы >>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>> -->
            <xsl:apply-templates select="//TS84" />
          </Table>
        </Worksheet>
      </xsl:when>
      <xsl:when test="TS9 = 1">
        <!-- <<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<< Лист ТХ9 >>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>> -->
        <Worksheet ss:Name="ТХ9">
          <Table>
            <Column ss:Width="90" />
            <Column ss:Width="80.25" />
            <Column ss:Width="83.25" />
            <Column ss:Index="4" ss:Width="51.75" />
            <Column ss:Width="37.5" />
            <Column ss:Index="8" ss:Width="53.25" />
            <Column ss:Width="53.25" />
            <Column ss:Index="11" ss:Width="53.25" />
            <Column ss:Width="53.25" />
            <Column ss:Width="60" />
            <Column ss:Width="57.75" />
            <Column ss:Index="16" ss:Width="48.75" />
            <Column ss:Width="63" />
            <Column ss:Width="53.25" />
            <Column ss:Index="20" ss:Width="61.5" />
            <Column ss:Width="70.5" />
            <Column ss:Width="55.5" />
            <Column ss:Width="114.75" />
            <!-- <<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<< Заголовок >>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>> -->
            <Row>
              <Cell ss:Index="11" ss:StyleID="TS9Title"><Data ss:Type="String">Расчет технологических потерь тепла при передаче тепловой энергии</Data></Cell>
            </Row>
            <Row>
              <Cell ss:Index="11" ss:StyleID="TS9Title"><Data ss:Type="String">по предприятию ГУП "ЖКХ РС(Я)"</Data></Cell>
            </Row>
            <Row>
              <Cell ss:Index="11" ss:StyleID="TS9Title"><Data ss:Type="String">на год</Data></Cell>
            </Row>
            <!-- <<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<< Столбцы таблицы >>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>> -->
            <Row ss:Height="42" ss:Index="5">
              <Cell ss:MergeDown="1" ss:StyleID="TS9Column"><Data ss:Type="String">Населенный пункт</Data></Cell>
              <Cell ss:MergeDown="1" ss:StyleID="TS9Column"><Data ss:Type="String">Наименование котельной</Data></Cell>
              <Cell ss:MergeDown="1" ss:StyleID="TS9Column"><Data ss:Type="String">Участки тепловых сетей (адресная принадлежность)</Data></Cell>
              <Cell ss:MergeDown="1" ss:StyleID="TS9Column"><Data ss:Type="String">Диаметр трубы, мм</Data></Cell>
              <Cell ss:MergeDown="1" ss:StyleID="TS9Column"><Data ss:Type="String">Протяженность, м</Data></Cell>
              <Cell ss:MergeDown="1" ss:StyleID="TS9Column"><Data ss:Type="String">К-т</Data></Cell>
              <Cell ss:MergeAcross="1" ss:StyleID="TS9Column"><Data ss:Type="String">Уд. потери, ккал/м*ч</Data></Cell>
              <Cell ss:MergeAcross="1" ss:StyleID="TS9Column"><Data ss:Type="String">Часовые тепловые потери Q, Гкал/ч</Data></Cell>
              <Cell ss:MergeDown="1" ss:StyleID="TS9Column"><Data ss:Type="String">Кол-во часов работы, час</Data></Cell>
              <Cell ss:MergeAcross="1" ss:StyleID="TS9Column"><Data ss:Type="String">Годовые тепловые потери, Гкал</Data></Cell>
              <Cell ss:MergeDown="1" ss:StyleID="TS9Column"><Data ss:Type="String">Площадь попереч. сечения, м2</Data></Cell>
              <Cell ss:MergeAcross="2" ss:StyleID="TS9Column"><Data ss:Type="String">Объем теплоносителя V, м3</Data></Cell>
              <Cell ss:MergeAcross="3" ss:StyleID="TS9Column"><Data ss:Type="String">Потери теплоносителя G, м3</Data></Cell>
              <Cell ss:MergeAcross="2" ss:StyleID="TS9Column"><Data ss:Type="String">Тепловые потери Q, Гкал</Data></Cell>
            </Row>
            <Row ss:Height="38.25">
              <Cell ss:Index="7" ss:StyleID="TS9Column"><Data ss:Type="String">Под.</Data></Cell>
              <Cell ss:StyleID="TS9Column"><Data ss:Type="String">Обр.</Data></Cell>
              <Cell ss:StyleID="TS9Column"><Data ss:Type="String">Под.</Data></Cell>
              <Cell ss:StyleID="TS9Column"><Data ss:Type="String">Обр.</Data></Cell>
              <Cell ss:Index="12" ss:StyleID="TS9Column"><Data ss:Type="String">Под.</Data></Cell>
              <Cell ss:StyleID="TS9Column"><Data ss:Type="String">Обр.</Data></Cell>
              <Cell ss:Index="15" ss:StyleID="TS9Column"><Data ss:Type="String">Зимн.</Data></Cell>
              <Cell ss:StyleID="TS9Column"><Data ss:Type="String">Летн.</Data></Cell>
              <Cell ss:StyleID="TS9Column"><Data ss:Type="String">Ср. год.</Data></Cell>
              <Cell ss:StyleID="TS9Column"><Data ss:Type="String">С утечк.</Data></Cell>
              <Cell ss:StyleID="TS9Column"><Data ss:Type="String">На заполн.</Data></Cell>
              <Cell ss:StyleID="TS9Column"><Data ss:Type="String">На регл. раб.</Data></Cell>
              <Cell ss:StyleID="TS9Column"><Data ss:Type="String">Итого</Data></Cell>
              <Cell ss:StyleID="TS9Column"><Data ss:Type="String">С утечк.</Data></Cell>
              <Cell ss:StyleID="TS9Column"><Data ss:Type="String">На зап. и регл. работы</Data></Cell>
              <Cell ss:StyleID="TS9Column"><Data ss:Type="String">Общ.</Data></Cell>
            </Row>
            <Row>
              <Cell ss:StyleID="TS9Column"><Data ss:Type="Number">1</Data></Cell>
              <Cell ss:StyleID="TS9Column"><Data ss:Type="Number">2</Data></Cell>
              <Cell ss:StyleID="TS9Column"><Data ss:Type="Number">3</Data></Cell>
              <Cell ss:StyleID="TS9Column"><Data ss:Type="Number">4</Data></Cell>
              <Cell ss:StyleID="TS9Column"><Data ss:Type="Number">5</Data></Cell>
              <Cell ss:StyleID="TS9Column"><Data ss:Type="Number">6</Data></Cell>
              <Cell ss:StyleID="TS9Column"><Data ss:Type="Number">7</Data></Cell>
              <Cell ss:StyleID="TS9Column"><Data ss:Type="Number">8</Data></Cell>
              <Cell ss:StyleID="TS9Column"><Data ss:Type="Number">9</Data></Cell>
              <Cell ss:StyleID="TS9Column"><Data ss:Type="Number">10</Data></Cell>
              <Cell ss:StyleID="TS9Column"><Data ss:Type="Number">11</Data></Cell>
              <Cell ss:StyleID="TS9Column"><Data ss:Type="Number">12</Data></Cell>
              <Cell ss:StyleID="TS9Column"><Data ss:Type="Number">13</Data></Cell>
              <Cell ss:StyleID="TS9Column"><Data ss:Type="Number">14</Data></Cell>
              <Cell ss:StyleID="TS9Column"><Data ss:Type="Number">15</Data></Cell>
              <Cell ss:StyleID="TS9Column"><Data ss:Type="Number">16</Data></Cell>
              <Cell ss:StyleID="TS9Column"><Data ss:Type="Number">17</Data></Cell>
              <Cell ss:StyleID="TS9Column"><Data ss:Type="Number">18</Data></Cell>
              <Cell ss:StyleID="TS9Column"><Data ss:Type="Number">19</Data></Cell>
              <Cell ss:StyleID="TS9Column"><Data ss:Type="Number">20</Data></Cell>
              <Cell ss:StyleID="TS9Column"><Data ss:Type="Number">21</Data></Cell>
              <Cell ss:StyleID="TS9Column"><Data ss:Type="Number">22</Data></Cell>
              <Cell ss:StyleID="TS9Column"><Data ss:Type="Number">23</Data></Cell>
              <Cell ss:StyleID="TS9Column"><Data ss:Type="Number">24</Data></Cell>
            </Row>
            <!-- <<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<< Данные таблицы >>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>> -->
            <xsl:apply-templates select="//TS9" />
          </Table>
        </Worksheet>
      </xsl:when>
      <xsl:when test="TS10 = 1">
        <!-- <<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<< Лист ТХ10 >>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>> -->
        <Worksheet ss:Name="ТХ10">
          <Table>
            <Column ss:Width="129.75" />
            <Column ss:Width="57" />
            <Column ss:Width="63" />
            <Column ss:Width="59.25" />
            <Column ss:Width="61.5" />
            <Column ss:Span="11" ss:Width="57" />
            <!-- <<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<< Заголовок >>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>> -->
            <Row>
              <Cell ss:Index="6" ss:StyleID="TS10Title"><Data ss:Type="String">Расчет расхода воды на технологические нужды и нормативные утечки</Data></Cell>
              <Cell ss:Index="15" ss:StyleID="TS10Title"><Data ss:Type="String">Форма 10 (ТХ)</Data></Cell>
            </Row>
            <Row>
              <Cell ss:Index="6" ss:StyleID="TS10Title"><Data ss:Type="String">по филиалу-участку</Data></Cell>
            </Row>
            <Row>
              <Cell ss:Index="6" ss:StyleID="TS10Title"><Data ss:Type="String">на год</Data></Cell>
            </Row>
            <!-- <<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<< Столбцы таблицы >>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>> -->
            <Row ss:Index="6">
              <Cell ss:MergeDown="1" ss:StyleID="TS10Column"><Data ss:Type="String">Производственные подразделения теплоэнергетического хоз-ва (участок, цех)</Data></Cell>
              <Cell ss:MergeAcross="4" ss:StyleID="TS10Column"><Data ss:Type="String">Разовое наполнение систем отопления</Data></Cell>
              <Cell ss:MergeAcross="5" ss:StyleID="TS10Column"><Data ss:Type="String">Наполнение т/сетей</Data></Cell>
              <Cell ss:MergeAcross="1" ss:StyleID="TS10Column"><Data ss:Type="String">Утечки воды</Data></Cell>
              <Cell ss:MergeDown="1" ss:StyleID="TS10Column"><Data ss:Type="String">Общий расход воды, м3</Data></Cell>
              <Cell ss:MergeDown="1" ss:StyleID="TS10Column"><Data ss:Type="String">Слив из системы отопления</Data></Cell>
              <Cell ss:MergeDown="1" ss:StyleID="TS10Column"><Data ss:Type="String">ХВС</Data></Cell>
            </Row>
            <Row ss:Height="72.5">
              <Cell ss:Index="2" ss:StyleID="TS10Column"><Data ss:Type="String">Характер теплоснабж. системы</Data></Cell>
              <Cell ss:StyleID="TS10Column"><Data ss:Type="String">Перепад температ. в сист. теплопотреблен.</Data></Cell>
              <Cell ss:StyleID="TS10Column"><Data ss:Type="String">Удельный объем воды</Data></Cell>
              <Cell ss:StyleID="TS10Column"><Data ss:Type="String">Расчетная тепловая нагрузка, Гкал/час</Data></Cell>
              <Cell ss:StyleID="TS10Column"><Data ss:Type="String">Расход воды, м3</Data></Cell>
              <Cell ss:StyleID="TS10Column"><Data ss:Type="String">Протяжен. т/сетей, км</Data></Cell>
              <Cell ss:StyleID="TS10Column"><Data ss:Type="String">Наружн. диаметр усл. прох., мм</Data></Cell>
              <Cell ss:StyleID="TS10Column"><Data ss:Type="String">Внутрен. диаметр, мм</Data></Cell>
              <Cell ss:StyleID="TS10Column"><Data ss:Type="String">Толщина стенки, мм</Data></Cell>
              <Cell ss:StyleID="TS10Column"><Data ss:Type="String">Удельный объем воды, м3/км</Data></Cell>
              <Cell ss:StyleID="TS10Column"><Data ss:Type="String">Расход воды, м3</Data></Cell>
              <Cell ss:StyleID="TS10Column"><Data ss:Type="String">Расход воды за час</Data></Cell>
              <Cell ss:StyleID="TS10Column"><Data ss:Type="String">Расход воды на утечки</Data></Cell>
            </Row>
            <Row>
              <Cell ss:StyleID="TS10NumColumn"><Data ss:Type="Number">1</Data></Cell>
              <Cell ss:StyleID="TS10NumColumn"><Data ss:Type="Number">2</Data></Cell>
              <Cell ss:StyleID="TS10NumColumn"><Data ss:Type="Number">3</Data></Cell>
              <Cell ss:StyleID="TS10NumColumn"><Data ss:Type="Number">4</Data></Cell>
              <Cell ss:StyleID="TS10NumColumn"><Data ss:Type="Number">5</Data></Cell>
              <Cell ss:StyleID="TS10NumColumn"><Data ss:Type="Number">6</Data></Cell>
              <Cell ss:StyleID="TS10NumColumn"><Data ss:Type="Number">7</Data></Cell>
              <Cell ss:StyleID="TS10NumColumn"><Data ss:Type="Number">8</Data></Cell>
              <Cell ss:StyleID="TS10NumColumn"><Data ss:Type="Number">9</Data></Cell>
              <Cell ss:StyleID="TS10NumColumn"><Data ss:Type="Number">10</Data></Cell>
              <Cell ss:StyleID="TS10NumColumn"><Data ss:Type="Number">11</Data></Cell>
              <Cell ss:StyleID="TS10NumColumn"><Data ss:Type="Number">12</Data></Cell>
              <Cell ss:StyleID="TS10NumColumn"><Data ss:Type="Number">13</Data></Cell>
              <Cell ss:StyleID="TS10NumColumn"><Data ss:Type="Number">14</Data></Cell>
              <Cell ss:StyleID="TS10NumColumn"><Data ss:Type="Number">15</Data></Cell>
              <Cell ss:StyleID="TS10NumColumn"><Data ss:Type="Number">16</Data></Cell>
              <Cell ss:StyleID="TS10NumColumn"><Data ss:Type="Number">17</Data></Cell>
            </Row>
            <!-- <<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<< Данные таблицы >>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>> -->
            <xsl:apply-templates select="//TS10" />
          </Table>
        </Worksheet>
      </xsl:when>
    </xsl:choose>
  </xsl:template>
  
  <!-- <<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<< Шаблон данных листа ТХ32 >>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>> -->
    <xsl:template match="TS32">
    <xsl:choose>
      <xsl:when test="object_address = 'ВСЕГО:'">
        <Row ss:Height="12.75" ss:StyleID="s293a">
           
          <Cell ss:StyleID="s294a"><Data ss:Type="String"><xsl:value-of select="number"/></Data></Cell>
          <Cell ss:StyleID="s295a"><Data ss:Type="String"><xsl:value-of select="object_address"/></Data></Cell>
          <Cell ss:StyleID="s295a"><Data ss:Type="String"><xsl:value-of select="region_name"/></Data></Cell>
          <Cell ss:StyleID="s295a"><Data ss:Type="String"><xsl:value-of select="municipal_name"/></Data></Cell>
          <Cell ss:StyleID="s295a"><Data ss:Type="String"><xsl:value-of select="city_name"/></Data></Cell>
          <Cell ss:StyleID="s295a"><Data ss:Type="String"><xsl:value-of select="boiler_name"/></Data></Cell>
          <Cell ss:StyleID="s295a"><Data ss:Type="String"><xsl:value-of select="boiler_num"/></Data></Cell>
          <Cell ss:StyleID="s294a"><Data ss:Type="Number"><xsl:value-of select="input_num"/></Data></Cell>
          <Cell ss:StyleID="s294a"><Data ss:Type="String"><xsl:value-of select="input_date"/></Data></Cell>
          <Cell ss:StyleID="s294a"><Data ss:Type="String"><xsl:value-of select="plp_odn"/></Data></Cell>
          <Cell ss:StyleID="s294a"><Data ss:Type="String"><xsl:value-of select="power_total"/></Data></Cell>
          <Cell ss:StyleID="s294a"><Data ss:Type="Number"><xsl:value-of select="power_ccw"/></Data></Cell>
          <Cell ss:StyleID="s295a"><Data ss:Type="Number"><xsl:value-of select="power_cow"/></Data></Cell>
          <Cell ss:StyleID="s295a"><Data ss:Type="Number"><xsl:value-of select="power_codn"/></Data></Cell>
          <Cell ss:StyleID="s295a"><Data ss:Type="Number"><xsl:value-of select="power_hcw"/></Data></Cell>
          <Cell ss:StyleID="s295a"><Data ss:Type="Number"><xsl:value-of select="power_how"/></Data></Cell>
          <Cell ss:StyleID="s295a"><Data ss:Type="Number"><xsl:value-of select="power_hto"/></Data></Cell>
          <Cell ss:StyleID="s295a"><Data ss:Type="String"><xsl:value-of select="power_hodn"/></Data></Cell>
          <Cell ss:StyleID="s295a"><Data ss:Type="Number"><xsl:value-of select="appr_ac"/></Data></Cell>
          <Cell ss:StyleID="s295a"><Data ss:Type="Number"><xsl:value-of select="appr_people_num"/></Data></Cell>
          <Cell ss:StyleID="s295a"><Data ss:Type="Number"><xsl:value-of select="appr_total_vol"/></Data></Cell>
          <Cell ss:StyleID="s295a"><Data ss:Type="Number"><xsl:value-of select="appr_vol_ccw"/></Data></Cell>
          <Cell ss:StyleID="s295a"><Data ss:Type="Number"><xsl:value-of select="appr_vol_cow"/></Data></Cell>
          <Cell ss:StyleID="s295a"><Data ss:Type="Number"><xsl:value-of select="appr_vol_codn"/></Data></Cell>
          <Cell ss:StyleID="s295a"><Data ss:Type="Number"><xsl:value-of select="appr_vol_hcw"/></Data></Cell>
          
          <Cell ss:StyleID="s295a"><Data ss:Type="Number"><xsl:value-of select="appr_vol_how"/></Data></Cell>
          <Cell ss:StyleID="s295a"><Data ss:Type="Number"><xsl:value-of select="appr_vol_hto"/></Data></Cell>
          <Cell ss:StyleID="s295a"><Data ss:Type="Number"><xsl:value-of select="appr_vol_hodn"/></Data></Cell>
          
          
          
          <Cell ss:StyleID="s295a"><Data ss:Type="String"><xsl:value-of select="prog_ac"/></Data></Cell>
          <Cell ss:StyleID="s295a"><Data ss:Type="Number"><xsl:value-of select="prog_people_num"/></Data></Cell>
          <Cell ss:StyleID="s295a"><Data ss:Type="Number"><xsl:value-of select="prog_total_val"/></Data></Cell>
          <Cell ss:StyleID="s295a"><Data ss:Type="Number"><xsl:value-of select="prog_vol_ccw"/></Data></Cell>
          <Cell ss:StyleID="s295a"><Data ss:Type="Number"><xsl:value-of select="prog_vol_cow"/></Data></Cell>
          <Cell ss:StyleID="s295a"><Data ss:Type="Number"><xsl:value-of select="prog_vol_codn"/></Data></Cell>
          <Cell ss:StyleID="s295a"><Data ss:Type="Number"><xsl:value-of select="prog_vol_hcw"/></Data></Cell>
          <Cell ss:StyleID="s295a"><Data ss:Type="Number"><xsl:value-of select="prog_vol_how"/></Data></Cell>
          <Cell ss:StyleID="s295a"><Data ss:Type="Number"><xsl:value-of select="prog_vol_hto"/></Data></Cell>
          <Cell ss:StyleID="s295a"><Data ss:Type="Number"><xsl:value-of select="prog_vol_hodn"/></Data></Cell>
          
          <Cell ss:StyleID="s295a"><Data ss:Type="String"></Data></Cell>
          <Cell ss:StyleID="s295a" ss:Formula="=RC[-14]-RC[-28]"><Data ss:Type="Number"></Data></Cell>
          <Cell ss:StyleID="s295a"><Data ss:Type="String"></Data></Cell>
          <Cell ss:StyleID="s295a"><Data ss:Type="String"></Data></Cell>
          <Cell ss:StyleID="s295a"><Data ss:Type="String"></Data></Cell>
          <Cell ss:StyleID="s295a"><Data ss:Type="String"></Data></Cell>

           
        </Row>
      </xsl:when>
      <xsl:when test="number = 1 or number = 2 or number = 3 or number = 4 or number = 5 or number = 6">
        <Row ss:Height="12.75" ss:StyleID="s293a">
          
          <Cell ss:StyleID="s297a"><Data ss:Type="String"><xsl:value-of select="number"/></Data></Cell>
          <Cell ss:StyleID="s297a"><Data ss:Type="String"><xsl:value-of select="object_address"/></Data></Cell>
          <Cell ss:StyleID="s297a"><Data ss:Type="String"><xsl:value-of select="region_name"/></Data></Cell>
          <Cell ss:StyleID="s297a"><Data ss:Type="String"><xsl:value-of select="municipal_name"/></Data></Cell>
          <Cell ss:StyleID="s297a"><Data ss:Type="String"><xsl:value-of select="city_name"/></Data></Cell>
          <Cell ss:StyleID="s297a"><Data ss:Type="String"><xsl:value-of select="boiler_name"/></Data></Cell>
          <Cell ss:StyleID="s297a"><Data ss:Type="String"><xsl:value-of select="boiler_num"/></Data></Cell>
          <Cell ss:StyleID="s297a"><Data ss:Type="Number"><xsl:value-of select="input_num"/></Data></Cell>
          <Cell ss:StyleID="s297a"><Data ss:Type="String"><xsl:value-of select="input_date"/></Data></Cell>
          <Cell ss:StyleID="s297a"><Data ss:Type="String"><xsl:value-of select="plp_odn"/></Data></Cell>
          <Cell ss:StyleID="s297a"><Data ss:Type="String"><xsl:value-of select="power_total"/></Data></Cell>
          <Cell ss:StyleID="s297a"><Data ss:Type="Number"><xsl:value-of select="power_ccw"/></Data></Cell>
          <Cell ss:StyleID="s297a"><Data ss:Type="Number"><xsl:value-of select="power_cow"/></Data></Cell>
          <Cell ss:StyleID="s297a"><Data ss:Type="Number"><xsl:value-of select="power_codn"/></Data></Cell>
          <Cell ss:StyleID="s297a"><Data ss:Type="Number"><xsl:value-of select="power_hcw"/></Data></Cell>
          <Cell ss:StyleID="s297a"><Data ss:Type="Number"><xsl:value-of select="power_how"/></Data></Cell>
          <Cell ss:StyleID="s297a"><Data ss:Type="Number"><xsl:value-of select="power_hto"/></Data></Cell>
          <Cell ss:StyleID="s297a"><Data ss:Type="String"><xsl:value-of select="power_hodn"/></Data></Cell>
          <Cell ss:StyleID="s297a"><Data ss:Type="Number"><xsl:value-of select="appr_ac"/></Data></Cell>
          <Cell ss:StyleID="s297a"><Data ss:Type="Number"><xsl:value-of select="appr_people_num"/></Data></Cell>
          <Cell ss:StyleID="s297a"><Data ss:Type="Number"><xsl:value-of select="appr_total_vol"/></Data></Cell>
          <Cell ss:StyleID="s297a"><Data ss:Type="Number"><xsl:value-of select="appr_vol_ccw"/></Data></Cell>
          <Cell ss:StyleID="s297a"><Data ss:Type="Number"><xsl:value-of select="appr_vol_cow"/></Data></Cell>
          <Cell ss:StyleID="s297a"><Data ss:Type="Number"><xsl:value-of select="appr_vol_codn"/></Data></Cell>
          <Cell ss:StyleID="s297a"><Data ss:Type="Number"><xsl:value-of select="appr_vol_hcw"/></Data></Cell>
          
          <Cell ss:StyleID="s297a"><Data ss:Type="Number"><xsl:value-of select="appr_vol_how"/></Data></Cell>
          <Cell ss:StyleID="s297a"><Data ss:Type="Number"><xsl:value-of select="appr_vol_hto"/></Data></Cell>
          <Cell ss:StyleID="s297a"><Data ss:Type="Number"><xsl:value-of select="appr_vol_hodn"/></Data></Cell>
          
          
          
          <Cell ss:StyleID="s297a"><Data ss:Type="String"><xsl:value-of select="prog_ac"/></Data></Cell>
          <Cell ss:StyleID="s297a"><Data ss:Type="Number"><xsl:value-of select="prog_people_num"/></Data></Cell>
          <Cell ss:StyleID="s297a"><Data ss:Type="Number"><xsl:value-of select="prog_total_val"/></Data></Cell>
          <Cell ss:StyleID="s297a"><Data ss:Type="Number"><xsl:value-of select="prog_vol_ccw"/></Data></Cell>
          <Cell ss:StyleID="s297a"><Data ss:Type="Number"><xsl:value-of select="prog_vol_cow"/></Data></Cell>
          <Cell ss:StyleID="s297a"><Data ss:Type="Number"><xsl:value-of select="prog_vol_codn"/></Data></Cell>
          <Cell ss:StyleID="s297a"><Data ss:Type="Number"><xsl:value-of select="prog_vol_hcw"/></Data></Cell>
          <Cell ss:StyleID="s297a"><Data ss:Type="Number"><xsl:value-of select="prog_vol_how"/></Data></Cell>
          <Cell ss:StyleID="s297a"><Data ss:Type="Number"><xsl:value-of select="prog_vol_hto"/></Data></Cell>
          <Cell ss:StyleID="s297a"><Data ss:Type="Number"><xsl:value-of select="prog_vol_hodn"/></Data></Cell>
          <Cell ss:StyleID="s297a"><Data ss:Type="String"></Data></Cell>
          <Cell ss:StyleID="s297a" ss:Formula="=RC[-14]-RC[-28]"><Data ss:Type="Number"></Data></Cell>
          <Cell ss:StyleID="s297a"><Data ss:Type="String"></Data></Cell>
          <Cell ss:StyleID="s297a"><Data ss:Type="String"></Data></Cell>
          <Cell ss:StyleID="s297a"><Data ss:Type="String"></Data></Cell>
          <Cell ss:StyleID="s297a"><Data ss:Type="String"></Data></Cell>
          
        </Row>
      </xsl:when>
      <xsl:when test="object_address = 'Прочие' or object_address = 'Культура' or object_address = 'Образование' or object_address = 'Прочие' or object_address = 'Муниципальный фонд' or object_address = 'Ч/сектор'">
        <Row ss:Height="12.75" ss:StyleID="s293a">
          
          <Cell ss:StyleID="s294a"><Data ss:Type="String"><xsl:value-of select="number"/></Data></Cell>
          <Cell ss:StyleID="s295a"><Data ss:Type="String"><xsl:value-of select="object_address"/></Data></Cell>
          <Cell ss:StyleID="s295a"><Data ss:Type="String"><xsl:value-of select="region_name"/></Data></Cell>
          <Cell ss:StyleID="s295a"><Data ss:Type="String"><xsl:value-of select="municipal_name"/></Data></Cell>
          <Cell ss:StyleID="s295a"><Data ss:Type="String"><xsl:value-of select="city_name"/></Data></Cell>
          <Cell ss:StyleID="s295a"><Data ss:Type="String"><xsl:value-of select="boiler_name"/></Data></Cell>
          <Cell ss:StyleID="s295a"><Data ss:Type="String"><xsl:value-of select="boiler_num"/></Data></Cell>
          <Cell ss:StyleID="s294a"><Data ss:Type="Number"><xsl:value-of select="input_num"/></Data></Cell>
          <Cell ss:StyleID="s294a"><Data ss:Type="String"><xsl:value-of select="input_date"/></Data></Cell>
          <Cell ss:StyleID="s294a"><Data ss:Type="String"><xsl:value-of select="plp_odn"/></Data></Cell>
          <Cell ss:StyleID="s294a"><Data ss:Type="String"><xsl:value-of select="power_total"/></Data></Cell>
          <Cell ss:StyleID="s294a"><Data ss:Type="Number"><xsl:value-of select="power_ccw"/></Data></Cell>
          <Cell ss:StyleID="s295a"><Data ss:Type="Number"><xsl:value-of select="power_cow"/></Data></Cell>
          <Cell ss:StyleID="s295a"><Data ss:Type="Number"><xsl:value-of select="power_codn"/></Data></Cell>
          <Cell ss:StyleID="s295a"><Data ss:Type="Number"><xsl:value-of select="power_hcw"/></Data></Cell>
          <Cell ss:StyleID="s295a"><Data ss:Type="Number"><xsl:value-of select="power_how"/></Data></Cell>
          <Cell ss:StyleID="s295a"><Data ss:Type="Number"><xsl:value-of select="power_hto"/></Data></Cell>
          <Cell ss:StyleID="s295a"><Data ss:Type="String"><xsl:value-of select="power_hodn"/></Data></Cell>
          <Cell ss:StyleID="s295a"><Data ss:Type="Number"><xsl:value-of select="appr_ac"/></Data></Cell>
          <Cell ss:StyleID="s295a"><Data ss:Type="Number"><xsl:value-of select="appr_people_num"/></Data></Cell>
          <Cell ss:StyleID="s295a"><Data ss:Type="Number"><xsl:value-of select="appr_total_vol"/></Data></Cell>
          <Cell ss:StyleID="s295a"><Data ss:Type="Number"><xsl:value-of select="appr_vol_ccw"/></Data></Cell>
          <Cell ss:StyleID="s295a"><Data ss:Type="Number"><xsl:value-of select="appr_vol_cow"/></Data></Cell>
          <Cell ss:StyleID="s295a"><Data ss:Type="Number"><xsl:value-of select="appr_vol_codn"/></Data></Cell>
          <Cell ss:StyleID="s295a"><Data ss:Type="Number"><xsl:value-of select="appr_vol_hcw"/></Data></Cell>
          
          <Cell ss:StyleID="s295a"><Data ss:Type="Number"><xsl:value-of select="appr_vol_how"/></Data></Cell>
          <Cell ss:StyleID="s295a"><Data ss:Type="Number"><xsl:value-of select="appr_vol_hto"/></Data></Cell>
          <Cell ss:StyleID="s295a"><Data ss:Type="Number"><xsl:value-of select="appr_vol_hodn"/></Data></Cell>
          
          
          
          <Cell ss:StyleID="s295a"><Data ss:Type="String"><xsl:value-of select="prog_ac"/></Data></Cell>
          <Cell ss:StyleID="s295a"><Data ss:Type="Number"><xsl:value-of select="prog_people_num"/></Data></Cell>
          <Cell ss:StyleID="s295a"><Data ss:Type="Number"><xsl:value-of select="prog_total_val"/></Data></Cell>
          <Cell ss:StyleID="s295a"><Data ss:Type="Number"><xsl:value-of select="prog_vol_ccw"/></Data></Cell>
          <Cell ss:StyleID="s295a"><Data ss:Type="Number"><xsl:value-of select="prog_vol_cow"/></Data></Cell>
          <Cell ss:StyleID="s295a"><Data ss:Type="Number"><xsl:value-of select="prog_vol_codn"/></Data></Cell>
          <Cell ss:StyleID="s295a"><Data ss:Type="Number"><xsl:value-of select="prog_vol_hcw"/></Data></Cell>
          <Cell ss:StyleID="s295a"><Data ss:Type="Number"><xsl:value-of select="prog_vol_how"/></Data></Cell>
          <Cell ss:StyleID="s295a"><Data ss:Type="Number"><xsl:value-of select="prog_vol_hto"/></Data></Cell>
          <Cell ss:StyleID="s295a"><Data ss:Type="Number"><xsl:value-of select="prog_vol_hodn"/></Data></Cell>
          <Cell ss:StyleID="s295a"><Data ss:Type="String"></Data></Cell>
          <Cell ss:StyleID="s295a" ss:Formula="=RC[-14]-RC[-28]"><Data ss:Type="Number"></Data></Cell>
          <Cell ss:StyleID="s295a"><Data ss:Type="String"></Data></Cell>
          <Cell ss:StyleID="s295a"><Data ss:Type="String"></Data></Cell>
          <Cell ss:StyleID="s295a"><Data ss:Type="String"></Data></Cell>
          <Cell ss:StyleID="s295a"><Data ss:Type="String"></Data></Cell>
          
        </Row>
      </xsl:when>
      <xsl:when test="object_address != ''">
        <Row ss:Height="12.75" ss:StyleID="s300a">
          
          <Cell ss:StyleID="s302a"><Data ss:Type="String"><xsl:value-of select="number"/></Data></Cell>
          <Cell ss:StyleID="s302a"><Data ss:Type="String"><xsl:value-of select="object_address"/></Data></Cell>
          
          <Cell ss:StyleID="s302a"><Data ss:Type="String"><xsl:value-of select="region_name"/></Data></Cell>
          
          <Cell ss:StyleID="s302a"><Data ss:Type="String"><xsl:value-of select="municipal_name"/></Data></Cell>
          <Cell ss:StyleID="s302a"><Data ss:Type="String"><xsl:value-of select="city_name"/></Data></Cell>
          <Cell ss:StyleID="s302a"><Data ss:Type="String"><xsl:value-of select="boiler_name"/></Data></Cell>
          <Cell ss:StyleID="s302a"><Data ss:Type="String"><xsl:value-of select="boiler_num"/></Data></Cell>
          <Cell ss:StyleID="s302a"><Data ss:Type="Number"><xsl:value-of select="input_num"/></Data></Cell>
          <Cell ss:StyleID="s302a"><Data ss:Type="String"><xsl:value-of select="input_date"/></Data></Cell>
          <Cell ss:StyleID="s302a"><Data ss:Type="String"><xsl:value-of select="plp_odn"/></Data></Cell>
          <Cell ss:StyleID="s302a"><Data ss:Type="String"><xsl:value-of select="power_total"/></Data></Cell>
          <Cell ss:StyleID="s302a"><Data ss:Type="Number"><xsl:value-of select="power_ccw"/></Data></Cell>
          <Cell ss:StyleID="s302a"><Data ss:Type="Number"><xsl:value-of select="power_cow"/></Data></Cell>
          <!--w -->
          <Cell ss:StyleID="s302a"><Data ss:Type="Number"><xsl:value-of select="power_codn"/></Data></Cell>
          <Cell ss:StyleID="s302a"><Data ss:Type="Number"><xsl:value-of select="power_hcw"/></Data></Cell>
          <Cell ss:StyleID="s302a"><Data ss:Type="Number"><xsl:value-of select="power_how"/></Data></Cell>
          <Cell ss:StyleID="s302a"><Data ss:Type="Number"><xsl:value-of select="power_hto"/></Data></Cell>
          <Cell ss:StyleID="s302a"><Data ss:Type="String"><xsl:value-of select="power_hodn"/></Data></Cell>
          <!--w-->
          <Cell ss:StyleID="s302a"><Data ss:Type="String"><xsl:value-of select="appr_ac"/></Data></Cell>
          <!--
          <Cell ss:StyleID="s302a"><Data ss:Type="Number"><xsl:value-of select="appr_ac"/></Data></Cell>
          -->
          <Cell ss:StyleID="s302a"><Data ss:Type="Number"><xsl:value-of select="appr_people_num"/></Data></Cell>
          
          <Cell ss:StyleID="s302a"><Data ss:Type="Number"><xsl:value-of select="appr_total_vol"/></Data></Cell>
          
          
          <Cell ss:StyleID="s302a"><Data ss:Type="Number"><xsl:value-of select="appr_vol_ccw"/></Data></Cell>
          <Cell ss:StyleID="s302a"><Data ss:Type="Number"><xsl:value-of select="appr_vol_cow"/></Data></Cell>
          <Cell ss:StyleID="s302a"><Data ss:Type="Number"><xsl:value-of select="appr_vol_codn"/></Data></Cell>
          <Cell ss:StyleID="s302a"><Data ss:Type="Number"><xsl:value-of select="appr_vol_hcw"/></Data></Cell>
          
          <Cell ss:StyleID="s302a"><Data ss:Type="Number"><xsl:value-of select="appr_vol_how"/></Data></Cell>
          <Cell ss:StyleID="s302a"><Data ss:Type="Number"><xsl:value-of select="appr_vol_hto"/></Data></Cell>
          <Cell ss:StyleID="s302a"><Data ss:Type="Number"><xsl:value-of select="appr_vol_hodn"/></Data></Cell>
          
          
          
          <Cell ss:StyleID="s302a"><Data ss:Type="String"><xsl:value-of select="prog_ac"/></Data></Cell>
          <Cell ss:StyleID="s302a"><Data ss:Type="Number"><xsl:value-of select="prog_people_num"/></Data></Cell>
          <Cell ss:StyleID="s302a"><Data ss:Type="Number"><xsl:value-of select="prog_total_val"/></Data></Cell>
          <Cell ss:StyleID="s302a"><Data ss:Type="Number"><xsl:value-of select="prog_vol_ccw"/></Data></Cell>
          <Cell ss:StyleID="s302a"><Data ss:Type="Number"><xsl:value-of select="prog_vol_cow"/></Data></Cell>
          <Cell ss:StyleID="s302a"><Data ss:Type="Number"><xsl:value-of select="prog_vol_codn"/></Data></Cell>
          <Cell ss:StyleID="s302a"><Data ss:Type="Number"><xsl:value-of select="prog_vol_hcw"/></Data></Cell>
          <Cell ss:StyleID="s302a"><Data ss:Type="Number"><xsl:value-of select="prog_vol_how"/></Data></Cell>
          <Cell ss:StyleID="s302a"><Data ss:Type="Number"><xsl:value-of select="prog_vol_hto"/></Data></Cell>
          <Cell ss:StyleID="s302a"><Data ss:Type="Number"><xsl:value-of select="prog_vol_hodn"/></Data></Cell>
          <Cell ss:StyleID="s302a"><Data ss:Type="String"></Data></Cell>
          <Cell ss:StyleID="s302a" ss:Formula="=RC[-14]-RC[-28]"><Data ss:Type="Number"></Data></Cell>
          <Cell ss:StyleID="s302a"><Data ss:Type="String"></Data></Cell>
          <Cell ss:StyleID="s302a"><Data ss:Type="String"></Data></Cell>
          <Cell ss:StyleID="s302a"><Data ss:Type="String"></Data></Cell>
          <Cell ss:StyleID="s302a"><Data ss:Type="String"></Data></Cell>
          
        </Row>
      </xsl:when>
    </xsl:choose>
  </xsl:template>
    
      
  <!-- <<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<< Шаблон данных листа ТХ12 >>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>> -->
    <xsl:template match="TS12">
    <xsl:choose>
      <xsl:when test="object_address = 'ВСЕГО:'">
        <Row ss:Height="12.75" ss:StyleID="s293a">
          <Cell ss:StyleID="s294a"><Data ss:Type="String"><xsl:value-of select="number"/></Data></Cell>
          <Cell ss:StyleID="s295a"><Data ss:Type="String"><xsl:value-of select="object_address"/></Data></Cell>
          <Cell ss:StyleID="s295a"><Data ss:Type="String"><xsl:value-of select="region_name"/></Data></Cell>
          <Cell ss:StyleID="s295a"><Data ss:Type="String"><xsl:value-of select="municipal_name"/></Data></Cell>
          <Cell ss:StyleID="s295a"><Data ss:Type="String"><xsl:value-of select="city_name"/></Data></Cell>
          <Cell ss:StyleID="s295a"><Data ss:Type="String"><xsl:value-of select="boiler_name"/></Data></Cell>
          <Cell ss:StyleID="s295a"><Data ss:Type="String"><xsl:value-of select="boiler_num"/></Data></Cell>
          <Cell ss:StyleID="s294a"><Data ss:Type="Number"><xsl:value-of select="input_num"/></Data></Cell>
          <Cell ss:StyleID="s294a"><Data ss:Type="String"><xsl:value-of select="input_date"/></Data></Cell>
          <Cell ss:StyleID="s294a"><Data ss:Type="String"><xsl:value-of select="plp_odn"/></Data></Cell>
          <Cell ss:StyleID="s294a"><Data ss:Type="String"><xsl:value-of select="power_total"/></Data></Cell>
          <Cell ss:StyleID="s294a"><Data ss:Type="Number"><xsl:value-of select="power_ccw"/></Data></Cell>
          <Cell ss:StyleID="s295a"><Data ss:Type="Number"><xsl:value-of select="power_cow"/></Data></Cell>
          <Cell ss:StyleID="s295a"><Data ss:Type="Number"><xsl:value-of select="power_codn"/></Data></Cell>
          <Cell ss:StyleID="s295a"><Data ss:Type="Number"><xsl:value-of select="power_hcw"/></Data></Cell>
          <Cell ss:StyleID="s295a"><Data ss:Type="Number"><xsl:value-of select="power_how"/></Data></Cell>
          <Cell ss:StyleID="s295a"><Data ss:Type="Number"><xsl:value-of select="power_hto"/></Data></Cell>
          <Cell ss:StyleID="s295a"><Data ss:Type="String"><xsl:value-of select="power_hodn"/></Data></Cell>
          <Cell ss:StyleID="s295a"><Data ss:Type="Number"><xsl:value-of select="appr_ac"/></Data></Cell>
          <Cell ss:StyleID="s295a"><Data ss:Type="Number"><xsl:value-of select="appr_people_num"/></Data></Cell>
          <Cell ss:StyleID="s295a"><Data ss:Type="Number"><xsl:value-of select="appr_total_vol"/></Data></Cell>
          <Cell ss:StyleID="s295a"><Data ss:Type="Number"><xsl:value-of select="appr_vol_ccw"/></Data></Cell>
          <Cell ss:StyleID="s295a"><Data ss:Type="Number"><xsl:value-of select="appr_vol_cow"/></Data></Cell>
          <Cell ss:StyleID="s295a"><Data ss:Type="Number"><xsl:value-of select="appr_vol_codn"/></Data></Cell>
          <Cell ss:StyleID="s295a"><Data ss:Type="Number"><xsl:value-of select="appr_vol_hcw"/></Data></Cell>
          
          <Cell ss:StyleID="s295a"><Data ss:Type="Number"><xsl:value-of select="appr_vol_how"/></Data></Cell>
          <Cell ss:StyleID="s295a"><Data ss:Type="Number"><xsl:value-of select="appr_vol_hto"/></Data></Cell>
          <Cell ss:StyleID="s295a"><Data ss:Type="Number"><xsl:value-of select="appr_vol_hodn"/></Data></Cell>
          
          
          
          <Cell ss:StyleID="s295a"><Data ss:Type="String"><xsl:value-of select="prog_ac"/></Data></Cell>
          <Cell ss:StyleID="s295a"><Data ss:Type="Number"><xsl:value-of select="prog_people_num"/></Data></Cell>
          <Cell ss:StyleID="s295a"><Data ss:Type="Number"><xsl:value-of select="prog_total_val"/></Data></Cell>
          <Cell ss:StyleID="s295a"><Data ss:Type="Number"><xsl:value-of select="prog_vol_ccw"/></Data></Cell>
          <Cell ss:StyleID="s295a"><Data ss:Type="Number"><xsl:value-of select="prog_vol_cow"/></Data></Cell>
          <Cell ss:StyleID="s295a"><Data ss:Type="Number"><xsl:value-of select="prog_vol_codn"/></Data></Cell>
          <Cell ss:StyleID="s295a"><Data ss:Type="Number"><xsl:value-of select="prog_vol_hcw"/></Data></Cell>
          <Cell ss:StyleID="s295a"><Data ss:Type="Number"><xsl:value-of select="prog_vol_how"/></Data></Cell>
          <Cell ss:StyleID="s295a"><Data ss:Type="Number"><xsl:value-of select="prog_vol_hto"/></Data></Cell>
          <Cell ss:StyleID="s295a"><Data ss:Type="Number"><xsl:value-of select="prog_vol_hodn"/></Data></Cell>
          
          <Cell ss:StyleID="s295a"><Data ss:Type="String"></Data></Cell>
          <Cell ss:StyleID="s295a" ss:Formula="=RC[-14]-RC[-28]"><Data ss:Type="Number"></Data></Cell>
          <Cell ss:StyleID="s295a"><Data ss:Type="String"></Data></Cell>
          <Cell ss:StyleID="s295a"><Data ss:Type="String"></Data></Cell>
          <Cell ss:StyleID="s295a"><Data ss:Type="String"></Data></Cell>
          <Cell ss:StyleID="s295a"><Data ss:Type="String"></Data></Cell>

          
        </Row>
      </xsl:when>
      <xsl:when test="number = 1 or number = 2 or number = 3 or number = 4 or number = 5 or number = 6">
        <Row ss:Height="12.75" ss:StyleID="s293a">
          <Cell ss:StyleID="s297a"><Data ss:Type="String"><xsl:value-of select="number"/></Data></Cell>
          <Cell ss:StyleID="s297a"><Data ss:Type="String"><xsl:value-of select="object_address"/></Data></Cell>
          <Cell ss:StyleID="s297a"><Data ss:Type="String"><xsl:value-of select="region_name"/></Data></Cell>
          <Cell ss:StyleID="s297a"><Data ss:Type="String"><xsl:value-of select="municipal_name"/></Data></Cell>
          <Cell ss:StyleID="s297a"><Data ss:Type="String"><xsl:value-of select="city_name"/></Data></Cell>
          <Cell ss:StyleID="s297a"><Data ss:Type="String"><xsl:value-of select="boiler_name"/></Data></Cell>
          <Cell ss:StyleID="s297a"><Data ss:Type="String"><xsl:value-of select="boiler_num"/></Data></Cell>
          <Cell ss:StyleID="s297a"><Data ss:Type="Number"><xsl:value-of select="input_num"/></Data></Cell>
          <Cell ss:StyleID="s297a"><Data ss:Type="String"><xsl:value-of select="input_date"/></Data></Cell>
          <Cell ss:StyleID="s297a"><Data ss:Type="String"><xsl:value-of select="plp_odn"/></Data></Cell>
          <Cell ss:StyleID="s297a"><Data ss:Type="String"><xsl:value-of select="power_total"/></Data></Cell>
          <Cell ss:StyleID="s297a"><Data ss:Type="Number"><xsl:value-of select="power_ccw"/></Data></Cell>
          <Cell ss:StyleID="s297a"><Data ss:Type="Number"><xsl:value-of select="power_cow"/></Data></Cell>
          <Cell ss:StyleID="s297a"><Data ss:Type="Number"><xsl:value-of select="power_codn"/></Data></Cell>
          <Cell ss:StyleID="s297a"><Data ss:Type="Number"><xsl:value-of select="power_hcw"/></Data></Cell>
          <Cell ss:StyleID="s297a"><Data ss:Type="Number"><xsl:value-of select="power_how"/></Data></Cell>
          <Cell ss:StyleID="s297a"><Data ss:Type="Number"><xsl:value-of select="power_hto"/></Data></Cell>
          <Cell ss:StyleID="s297a"><Data ss:Type="String"><xsl:value-of select="power_hodn"/></Data></Cell>
          <Cell ss:StyleID="s297a"><Data ss:Type="Number"><xsl:value-of select="appr_ac"/></Data></Cell>
          <Cell ss:StyleID="s297a"><Data ss:Type="Number"><xsl:value-of select="appr_people_num"/></Data></Cell>
          <Cell ss:StyleID="s297a"><Data ss:Type="Number"><xsl:value-of select="appr_total_vol"/></Data></Cell>
          <Cell ss:StyleID="s297a"><Data ss:Type="Number"><xsl:value-of select="appr_vol_ccw"/></Data></Cell>
          <Cell ss:StyleID="s297a"><Data ss:Type="Number"><xsl:value-of select="appr_vol_cow"/></Data></Cell>
          <Cell ss:StyleID="s297a"><Data ss:Type="Number"><xsl:value-of select="appr_vol_codn"/></Data></Cell>
          <Cell ss:StyleID="s297a"><Data ss:Type="Number"><xsl:value-of select="appr_vol_hcw"/></Data></Cell>
          
          <Cell ss:StyleID="s297a"><Data ss:Type="Number"><xsl:value-of select="appr_vol_how"/></Data></Cell>
          <Cell ss:StyleID="s297a"><Data ss:Type="Number"><xsl:value-of select="appr_vol_hto"/></Data></Cell>
          <Cell ss:StyleID="s297a"><Data ss:Type="Number"><xsl:value-of select="appr_vol_hodn"/></Data></Cell>
          
          
          
          <Cell ss:StyleID="s297a"><Data ss:Type="String"><xsl:value-of select="prog_ac"/></Data></Cell>
          <Cell ss:StyleID="s297a"><Data ss:Type="Number"><xsl:value-of select="prog_people_num"/></Data></Cell>
          <Cell ss:StyleID="s297a"><Data ss:Type="Number"><xsl:value-of select="prog_total_val"/></Data></Cell>
          <Cell ss:StyleID="s297a"><Data ss:Type="Number"><xsl:value-of select="prog_vol_ccw"/></Data></Cell>
          <Cell ss:StyleID="s297a"><Data ss:Type="Number"><xsl:value-of select="prog_vol_cow"/></Data></Cell>
          <Cell ss:StyleID="s297a"><Data ss:Type="Number"><xsl:value-of select="prog_vol_codn"/></Data></Cell>
          <Cell ss:StyleID="s297a"><Data ss:Type="Number"><xsl:value-of select="prog_vol_hcw"/></Data></Cell>
          <Cell ss:StyleID="s297a"><Data ss:Type="Number"><xsl:value-of select="prog_vol_how"/></Data></Cell>
          <Cell ss:StyleID="s297a"><Data ss:Type="Number"><xsl:value-of select="prog_vol_hto"/></Data></Cell>
          <Cell ss:StyleID="s297a"><Data ss:Type="Number"><xsl:value-of select="prog_vol_hodn"/></Data></Cell>
          <Cell ss:StyleID="s297a"><Data ss:Type="String"></Data></Cell>
          <Cell ss:StyleID="s297a" ss:Formula="=RC[-14]-RC[-28]"><Data ss:Type="Number"></Data></Cell>
          <Cell ss:StyleID="s297a"><Data ss:Type="String"></Data></Cell>
          <Cell ss:StyleID="s297a"><Data ss:Type="String"></Data></Cell>
          <Cell ss:StyleID="s297a"><Data ss:Type="String"></Data></Cell>
          <Cell ss:StyleID="s297a"><Data ss:Type="String"></Data></Cell>
        </Row>
      </xsl:when>
      <xsl:when test="object_address = 'Прочие' or object_address = 'Культура' or object_address = 'Образование' or object_address = 'Прочие' or object_address = 'Муниципальный фонд' or object_address = 'Ч/сектор'">
        <Row ss:Height="12.75" ss:StyleID="s293a">
          <Cell ss:StyleID="s294a"><Data ss:Type="String"><xsl:value-of select="number"/></Data></Cell>
          <Cell ss:StyleID="s295a"><Data ss:Type="String"><xsl:value-of select="object_address"/></Data></Cell>
          <Cell ss:StyleID="s295a"><Data ss:Type="String"><xsl:value-of select="region_name"/></Data></Cell>
          <Cell ss:StyleID="s295a"><Data ss:Type="String"><xsl:value-of select="municipal_name"/></Data></Cell>
          <Cell ss:StyleID="s295a"><Data ss:Type="String"><xsl:value-of select="city_name"/></Data></Cell>
          <Cell ss:StyleID="s295a"><Data ss:Type="String"><xsl:value-of select="boiler_name"/></Data></Cell>
          <Cell ss:StyleID="s295a"><Data ss:Type="String"><xsl:value-of select="boiler_num"/></Data></Cell>
          <Cell ss:StyleID="s294a"><Data ss:Type="Number"><xsl:value-of select="input_num"/></Data></Cell>
          <Cell ss:StyleID="s294a"><Data ss:Type="String"><xsl:value-of select="input_date"/></Data></Cell>
          <Cell ss:StyleID="s294a"><Data ss:Type="String"><xsl:value-of select="plp_odn"/></Data></Cell>
          <Cell ss:StyleID="s294a"><Data ss:Type="String"><xsl:value-of select="power_total"/></Data></Cell>
          <Cell ss:StyleID="s294a"><Data ss:Type="Number"><xsl:value-of select="power_ccw"/></Data></Cell>
          <Cell ss:StyleID="s295a"><Data ss:Type="Number"><xsl:value-of select="power_cow"/></Data></Cell>
          <Cell ss:StyleID="s295a"><Data ss:Type="Number"><xsl:value-of select="power_codn"/></Data></Cell>
          <Cell ss:StyleID="s295a"><Data ss:Type="Number"><xsl:value-of select="power_hcw"/></Data></Cell>
          <Cell ss:StyleID="s295a"><Data ss:Type="Number"><xsl:value-of select="power_how"/></Data></Cell>
          <Cell ss:StyleID="s295a"><Data ss:Type="Number"><xsl:value-of select="power_hto"/></Data></Cell>
          <Cell ss:StyleID="s295a"><Data ss:Type="String"><xsl:value-of select="power_hodn"/></Data></Cell>
          <Cell ss:StyleID="s295a"><Data ss:Type="Number"><xsl:value-of select="appr_ac"/></Data></Cell>
          <Cell ss:StyleID="s295a"><Data ss:Type="Number"><xsl:value-of select="appr_people_num"/></Data></Cell>
          <Cell ss:StyleID="s295a"><Data ss:Type="Number"><xsl:value-of select="appr_total_vol"/></Data></Cell>
          <Cell ss:StyleID="s295a"><Data ss:Type="Number"><xsl:value-of select="appr_vol_ccw"/></Data></Cell>
          <Cell ss:StyleID="s295a"><Data ss:Type="Number"><xsl:value-of select="appr_vol_cow"/></Data></Cell>
          <Cell ss:StyleID="s295a"><Data ss:Type="Number"><xsl:value-of select="appr_vol_codn"/></Data></Cell>
          <Cell ss:StyleID="s295a"><Data ss:Type="Number"><xsl:value-of select="appr_vol_hcw"/></Data></Cell>
          
          <Cell ss:StyleID="s295a"><Data ss:Type="Number"><xsl:value-of select="appr_vol_how"/></Data></Cell>
          <Cell ss:StyleID="s295a"><Data ss:Type="Number"><xsl:value-of select="appr_vol_hto"/></Data></Cell>
          <Cell ss:StyleID="s295a"><Data ss:Type="Number"><xsl:value-of select="appr_vol_hodn"/></Data></Cell>
          
          
          
          <Cell ss:StyleID="s295a"><Data ss:Type="String"><xsl:value-of select="prog_ac"/></Data></Cell>
          <Cell ss:StyleID="s295a"><Data ss:Type="Number"><xsl:value-of select="prog_people_num"/></Data></Cell>
          <Cell ss:StyleID="s295a"><Data ss:Type="Number"><xsl:value-of select="prog_total_val"/></Data></Cell>
          <Cell ss:StyleID="s295a"><Data ss:Type="Number"><xsl:value-of select="prog_vol_ccw"/></Data></Cell>
          <Cell ss:StyleID="s295a"><Data ss:Type="Number"><xsl:value-of select="prog_vol_cow"/></Data></Cell>
          <Cell ss:StyleID="s295a"><Data ss:Type="Number"><xsl:value-of select="prog_vol_codn"/></Data></Cell>
          <Cell ss:StyleID="s295a"><Data ss:Type="Number"><xsl:value-of select="prog_vol_hcw"/></Data></Cell>
          <Cell ss:StyleID="s295a"><Data ss:Type="Number"><xsl:value-of select="prog_vol_how"/></Data></Cell>
          <Cell ss:StyleID="s295a"><Data ss:Type="Number"><xsl:value-of select="prog_vol_hto"/></Data></Cell>
          <Cell ss:StyleID="s295a"><Data ss:Type="Number"><xsl:value-of select="prog_vol_hodn"/></Data></Cell>
          <Cell ss:StyleID="s295a"><Data ss:Type="String"></Data></Cell>
          <Cell ss:StyleID="s295a" ss:Formula="=RC[-14]-RC[-28]"><Data ss:Type="Number"></Data></Cell>
          <Cell ss:StyleID="s295a"><Data ss:Type="String"></Data></Cell>
          <Cell ss:StyleID="s295a"><Data ss:Type="String"></Data></Cell>
          <Cell ss:StyleID="s295a"><Data ss:Type="String"></Data></Cell>
          <Cell ss:StyleID="s295a"><Data ss:Type="String"></Data></Cell>
          
        </Row>
      </xsl:when>
      <xsl:when test="object_address != ''">
        <Row ss:Height="12.75" ss:StyleID="s300a">
          <Cell ss:StyleID="s302a"><Data ss:Type="String"><xsl:value-of select="number"/></Data></Cell>
          <Cell ss:StyleID="s302a"><Data ss:Type="String"><xsl:value-of select="object_address"/></Data></Cell>
          <Cell ss:StyleID="s302a"><Data ss:Type="String"><xsl:value-of select="region_name"/></Data></Cell>
          <Cell ss:StyleID="s302a"><Data ss:Type="String"><xsl:value-of select="municipal_name"/></Data></Cell>
          <Cell ss:StyleID="s302a"><Data ss:Type="String"><xsl:value-of select="city_name"/></Data></Cell>
          <Cell ss:StyleID="s302a"><Data ss:Type="String"><xsl:value-of select="boiler_name"/></Data></Cell>
          <Cell ss:StyleID="s302a"><Data ss:Type="String"><xsl:value-of select="boiler_num"/></Data></Cell>
          <Cell ss:StyleID="s302a"><Data ss:Type="Number"><xsl:value-of select="input_num"/></Data></Cell>
          <Cell ss:StyleID="s302a"><Data ss:Type="String"><xsl:value-of select="input_date"/></Data></Cell>
          <Cell ss:StyleID="s302a"><Data ss:Type="String"><xsl:value-of select="plp_odn"/></Data></Cell>
          <Cell ss:StyleID="s302a"><Data ss:Type="String"><xsl:value-of select="power_total"/></Data></Cell>
          <Cell ss:StyleID="s302a"><Data ss:Type="Number"><xsl:value-of select="power_ccw"/></Data></Cell>
          <Cell ss:StyleID="s302a"><Data ss:Type="Number"><xsl:value-of select="power_cow"/></Data></Cell>
          <Cell ss:StyleID="s302a"><Data ss:Type="Number"><xsl:value-of select="power_codn"/></Data></Cell>
          <Cell ss:StyleID="s302a"><Data ss:Type="Number"><xsl:value-of select="power_hcw"/></Data></Cell>
          <Cell ss:StyleID="s302a"><Data ss:Type="Number"><xsl:value-of select="power_how"/></Data></Cell>
          <Cell ss:StyleID="s302a"><Data ss:Type="Number"><xsl:value-of select="power_hto"/></Data></Cell>
          <Cell ss:StyleID="s302a"><Data ss:Type="String"><xsl:value-of select="power_hodn"/></Data></Cell>
          <Cell ss:StyleID="s302a"><Data ss:Type="Number"><xsl:value-of select="appr_ac"/></Data></Cell>
          <Cell ss:StyleID="s302a"><Data ss:Type="Number"><xsl:value-of select="appr_people_num"/></Data></Cell>
          <Cell ss:StyleID="s302a"><Data ss:Type="Number"><xsl:value-of select="appr_total_vol"/></Data></Cell>
          <Cell ss:StyleID="s302a"><Data ss:Type="Number"><xsl:value-of select="appr_vol_ccw"/></Data></Cell>
          <Cell ss:StyleID="s302a"><Data ss:Type="Number"><xsl:value-of select="appr_vol_cow"/></Data></Cell>
          <Cell ss:StyleID="s302a"><Data ss:Type="Number"><xsl:value-of select="appr_vol_codn"/></Data></Cell>
          <Cell ss:StyleID="s302a"><Data ss:Type="Number"><xsl:value-of select="appr_vol_hcw"/></Data></Cell>
          
          <Cell ss:StyleID="s302a"><Data ss:Type="Number"><xsl:value-of select="appr_vol_how"/></Data></Cell>
          <Cell ss:StyleID="s302a"><Data ss:Type="Number"><xsl:value-of select="appr_vol_hto"/></Data></Cell>
          <Cell ss:StyleID="s302a"><Data ss:Type="Number"><xsl:value-of select="appr_vol_hodn"/></Data></Cell>
          
          
          
          <Cell ss:StyleID="s302a"><Data ss:Type="String"><xsl:value-of select="prog_ac"/></Data></Cell>
          <Cell ss:StyleID="s302a"><Data ss:Type="Number"><xsl:value-of select="prog_people_num"/></Data></Cell>
          <Cell ss:StyleID="s302a"><Data ss:Type="Number"><xsl:value-of select="prog_total_val"/></Data></Cell>
          <Cell ss:StyleID="s302a"><Data ss:Type="Number"><xsl:value-of select="prog_vol_ccw"/></Data></Cell>
          <Cell ss:StyleID="s302a"><Data ss:Type="Number"><xsl:value-of select="prog_vol_cow"/></Data></Cell>
          <Cell ss:StyleID="s302a"><Data ss:Type="Number"><xsl:value-of select="prog_vol_codn"/></Data></Cell>
          <Cell ss:StyleID="s302a"><Data ss:Type="Number"><xsl:value-of select="prog_vol_hcw"/></Data></Cell>
          <Cell ss:StyleID="s302a"><Data ss:Type="Number"><xsl:value-of select="prog_vol_how"/></Data></Cell>
          <Cell ss:StyleID="s302a"><Data ss:Type="Number"><xsl:value-of select="prog_vol_hto"/></Data></Cell>
          <Cell ss:StyleID="s302a"><Data ss:Type="Number"><xsl:value-of select="prog_vol_hodn"/></Data></Cell>
          <Cell ss:StyleID="s302a"><Data ss:Type="String"></Data></Cell>
          <Cell ss:StyleID="s302a" ss:Formula="=RC[-14]-RC[-28]"><Data ss:Type="Number"></Data></Cell>
          <Cell ss:StyleID="s302a"><Data ss:Type="String"></Data></Cell>
          <Cell ss:StyleID="s302a"><Data ss:Type="String"></Data></Cell>
          <Cell ss:StyleID="s302a"><Data ss:Type="String"></Data></Cell>
          <Cell ss:StyleID="s302a"><Data ss:Type="String"></Data></Cell>
          
        </Row>
      </xsl:when>
    </xsl:choose>
  </xsl:template>
  <!-- <<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<< Шаблон данных листа ТХ11 >>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>> -->
  <xsl:template match="TS11">
    <xsl:choose>
      <xsl:when test="object_address = 'ВСЕГО:'">
        <Row ss:Height="12.75" ss:StyleID="s293a">
          <Cell ss:StyleID="s294a"><Data ss:Type="String"><xsl:value-of select="number"/></Data></Cell>
          <Cell ss:StyleID="s295a"><Data ss:Type="String"><xsl:value-of select="object_address"/></Data></Cell>
          <Cell ss:StyleID="s295a"><Data ss:Type="String"><xsl:value-of select="region_name"/></Data></Cell>
          <Cell ss:StyleID="s295a"><Data ss:Type="String"><xsl:value-of select="municipal_name"/></Data></Cell>
          <Cell ss:StyleID="s295a"><Data ss:Type="String"><xsl:value-of select="city_name"/></Data></Cell>
          <Cell ss:StyleID="s295a"><Data ss:Type="String"><xsl:value-of select="boiler_name"/></Data></Cell>
          <Cell ss:StyleID="s295a"><Data ss:Type="String"><xsl:value-of select="boiler_num"/></Data></Cell>
          <Cell ss:StyleID="s294a"><Data ss:Type="Number"><xsl:value-of select="input_num"/></Data></Cell>
          <Cell ss:StyleID="s294a"><Data ss:Type="String"><xsl:value-of select="input_date"/></Data></Cell>
          <Cell ss:StyleID="s294a"><Data ss:Type="String"><xsl:value-of select="power_total"/></Data></Cell>
          <Cell ss:StyleID="s294a"><Data ss:Type="Number"><xsl:value-of select="power_kk_cw"/></Data></Cell>
          <Cell ss:StyleID="s295a"><Data ss:Type="Number"><xsl:value-of select="power_kk_kcw"/></Data></Cell>
          <Cell ss:StyleID="s295a"><Data ss:Type="Number"><xsl:value-of select="power_kk_how"/></Data></Cell>
          <Cell ss:StyleID="s295a"><Data ss:Type="Number"><xsl:value-of select="power_kk_to"/></Data></Cell>
          <Cell ss:StyleID="s295a"><Data ss:Type="Number"><xsl:value-of select="power_vk_cw"/></Data></Cell>
          <Cell ss:StyleID="s295a"><Data ss:Type="Number"><xsl:value-of select="power_vk_hcw"/></Data></Cell>
          <Cell ss:StyleID="s295a"><Data ss:Type="Number"><xsl:value-of select="power_vk_how"/></Data></Cell>
          <Cell ss:StyleID="s295a"><Data ss:Type="String"><xsl:value-of select="power_vk_to"/></Data></Cell>
          <Cell ss:StyleID="s295a"><Data ss:Type="Number"><xsl:value-of select="appr_ac"/></Data></Cell>
          <Cell ss:StyleID="s295a"><Data ss:Type="Number"><xsl:value-of select="appr_people_num"/></Data></Cell>
          <Cell ss:StyleID="s295a"><Data ss:Type="Number"><xsl:value-of select="appr_total_vol"/></Data></Cell>
          <Cell ss:StyleID="s295a"><Data ss:Type="Number"><xsl:value-of select="appr_vol_kk_cw"/></Data></Cell>
          <Cell ss:StyleID="s295a"><Data ss:Type="Number"><xsl:value-of select="appr_vol_kk_hcw"/></Data></Cell>
          <Cell ss:StyleID="s295a"><Data ss:Type="Number"><xsl:value-of select="appr_vol_kk_how"/></Data></Cell>
          <Cell ss:StyleID="s295a"><Data ss:Type="Number"><xsl:value-of select="appr_vol_kk_to"/></Data></Cell>
          
          <Cell ss:StyleID="s295a"><Data ss:Type="Number"><xsl:value-of select="appr_vol_vk_cw"/></Data></Cell>
          <Cell ss:StyleID="s295a"><Data ss:Type="Number"><xsl:value-of select="appr_vol_vk_hcw"/></Data></Cell>
          <Cell ss:StyleID="s295a"><Data ss:Type="Number"><xsl:value-of select="appr_vol_vk_how"/></Data></Cell>
          <Cell ss:StyleID="s295a"><Data ss:Type="Number"><xsl:value-of select="appr_vol_vk_to"/></Data></Cell>
          
          
          <Cell ss:StyleID="s295a"><Data ss:Type="String"><xsl:value-of select="prog_ac"/></Data></Cell>
          <Cell ss:StyleID="s295a"><Data ss:Type="Number"><xsl:value-of select="prog_people_num"/></Data></Cell>
          <Cell ss:StyleID="s295a"><Data ss:Type="Number"><xsl:value-of select="prog_total_val"/></Data></Cell>
          <Cell ss:StyleID="s295a"><Data ss:Type="Number"><xsl:value-of select="prog_vol_kk_cw"/></Data></Cell>
          <Cell ss:StyleID="s295a"><Data ss:Type="Number"><xsl:value-of select="prog_vol_kk_hcw"/></Data></Cell>
          <Cell ss:StyleID="s295a"><Data ss:Type="Number"><xsl:value-of select="prog_vol_kk_how"/></Data></Cell>
          <Cell ss:StyleID="s295a"><Data ss:Type="Number"><xsl:value-of select="prog_vol_kk_to"/></Data></Cell>
          <Cell ss:StyleID="s295a"><Data ss:Type="Number"><xsl:value-of select="prog_vol_vk_cw"/></Data></Cell>
          <Cell ss:StyleID="s295a"><Data ss:Type="Number"><xsl:value-of select="prog_vol_vk_hcw"/></Data></Cell>
          <Cell ss:StyleID="s295a"><Data ss:Type="Number"><xsl:value-of select="prog_vol_vk_how"/></Data></Cell>
          <Cell ss:StyleID="s295a"><Data ss:Type="Number"><xsl:value-of select="prog_vol_vk_to"/></Data></Cell>
          <Cell ss:StyleID="s295a"><Data ss:Type="String"></Data></Cell>
          <Cell ss:StyleID="s295a" ss:Formula="=RC[-14]-RC[-28]"><Data ss:Type="Number"></Data></Cell>
          <Cell ss:StyleID="s295a"><Data ss:Type="String"></Data></Cell>
          <Cell ss:StyleID="s295a"><Data ss:Type="String"></Data></Cell>
          <Cell ss:StyleID="s295a"><Data ss:Type="String"></Data></Cell>
          <Cell ss:StyleID="s295a"><Data ss:Type="String"></Data></Cell>
        </Row>
      </xsl:when>
      <xsl:when test="number = 1 or number = 2 or number = 3 or number = 4 or number = 5 or number = 6">
        <Row ss:Height="12.75" ss:StyleID="s293a">
          <Cell ss:StyleID="s297a"><Data ss:Type="String"><xsl:value-of select="number"/></Data></Cell>
          <Cell ss:StyleID="s297a"><Data ss:Type="String"><xsl:value-of select="object_address"/></Data></Cell>
          <Cell ss:StyleID="s297a"><Data ss:Type="String"><xsl:value-of select="region_name"/></Data></Cell>
          <Cell ss:StyleID="s297a"><Data ss:Type="String"><xsl:value-of select="municipal_name"/></Data></Cell>
          <Cell ss:StyleID="s297a"><Data ss:Type="String"><xsl:value-of select="city_name"/></Data></Cell>
          <Cell ss:StyleID="s297a"><Data ss:Type="String"><xsl:value-of select="boiler_name"/></Data></Cell>
          <Cell ss:StyleID="s297a"><Data ss:Type="String"><xsl:value-of select="boiler_num"/></Data></Cell>
          <Cell ss:StyleID="s297a"><Data ss:Type="Number"><xsl:value-of select="input_num"/></Data></Cell>
          <Cell ss:StyleID="s297a"><Data ss:Type="String"><xsl:value-of select="input_date"/></Data></Cell>
          <Cell ss:StyleID="s297a"><Data ss:Type="String"><xsl:value-of select="power_total"/></Data></Cell>
          <Cell ss:StyleID="s297a"><Data ss:Type="Number"><xsl:value-of select="power_kk_cw"/></Data></Cell>
          <Cell ss:StyleID="s297a"><Data ss:Type="Number"><xsl:value-of select="power_kk_kcw"/></Data></Cell>
          <Cell ss:StyleID="s297a"><Data ss:Type="Number"><xsl:value-of select="power_kk_how"/></Data></Cell>
          <Cell ss:StyleID="s297a"><Data ss:Type="Number"><xsl:value-of select="power_kk_to"/></Data></Cell>
          <Cell ss:StyleID="s297a"><Data ss:Type="Number"><xsl:value-of select="power_vk_cw"/></Data></Cell>
          <Cell ss:StyleID="s297a"><Data ss:Type="Number"><xsl:value-of select="power_vk_hcw"/></Data></Cell>
          <Cell ss:StyleID="s297a"><Data ss:Type="Number"><xsl:value-of select="power_vk_how"/></Data></Cell>
          <Cell ss:StyleID="s297a"><Data ss:Type="String"><xsl:value-of select="power_vk_to"/></Data></Cell>
          <Cell ss:StyleID="s297a"><Data ss:Type="Number"><xsl:value-of select="appr_ac"/></Data></Cell>
          <Cell ss:StyleID="s297a"><Data ss:Type="Number"><xsl:value-of select="appr_people_num"/></Data></Cell>
          <Cell ss:StyleID="s297a"><Data ss:Type="Number"><xsl:value-of select="appr_total_vol"/></Data></Cell>
          <Cell ss:StyleID="s297a"><Data ss:Type="Number"><xsl:value-of select="appr_vol_kk_cw"/></Data></Cell>
          <Cell ss:StyleID="s297a"><Data ss:Type="Number"><xsl:value-of select="appr_vol_kk_hcw"/></Data></Cell>
          <Cell ss:StyleID="s297a"><Data ss:Type="Number"><xsl:value-of select="appr_vol_kk_how"/></Data></Cell>
          <Cell ss:StyleID="s297a"><Data ss:Type="Number"><xsl:value-of select="appr_vol_kk_to"/></Data></Cell>
          
          <Cell ss:StyleID="s297a"><Data ss:Type="Number"><xsl:value-of select="appr_vol_vk_cw"/></Data></Cell>
          <Cell ss:StyleID="s297a"><Data ss:Type="Number"><xsl:value-of select="appr_vol_vk_hcw"/></Data></Cell>
          <Cell ss:StyleID="s297a"><Data ss:Type="Number"><xsl:value-of select="appr_vol_vk_how"/></Data></Cell>
          <Cell ss:StyleID="s297a"><Data ss:Type="Number"><xsl:value-of select="appr_vol_vk_to"/></Data></Cell>
          
          
          <Cell ss:StyleID="s297a"><Data ss:Type="String"><xsl:value-of select="prog_ac"/></Data></Cell>
          <Cell ss:StyleID="s297a"><Data ss:Type="Number"><xsl:value-of select="prog_people_num"/></Data></Cell>
          <Cell ss:StyleID="s297a"><Data ss:Type="Number"><xsl:value-of select="prog_total_val"/></Data></Cell>
          <Cell ss:StyleID="s297a"><Data ss:Type="Number"><xsl:value-of select="prog_vol_kk_cw"/></Data></Cell>
          <Cell ss:StyleID="s297a"><Data ss:Type="Number"><xsl:value-of select="prog_vol_kk_hcw"/></Data></Cell>
          <Cell ss:StyleID="s297a"><Data ss:Type="Number"><xsl:value-of select="prog_vol_kk_how"/></Data></Cell>
          <Cell ss:StyleID="s297a"><Data ss:Type="Number"><xsl:value-of select="prog_vol_kk_to"/></Data></Cell>
          <Cell ss:StyleID="s297a"><Data ss:Type="Number"><xsl:value-of select="prog_vol_vk_cw"/></Data></Cell>
          <Cell ss:StyleID="s297a"><Data ss:Type="Number"><xsl:value-of select="prog_vol_vk_hcw"/></Data></Cell>
          <Cell ss:StyleID="s297a"><Data ss:Type="Number"><xsl:value-of select="prog_vol_vk_how"/></Data></Cell>
          <Cell ss:StyleID="s297a"><Data ss:Type="Number"><xsl:value-of select="prog_vol_vk_to"/></Data></Cell>
          <Cell ss:StyleID="s297a"><Data ss:Type="String"></Data></Cell>
          <Cell ss:StyleID="s297a" ss:Formula="=RC[-14]-RC[-28]"><Data ss:Type="Number"></Data></Cell>
          <Cell ss:StyleID="s297a"><Data ss:Type="String"></Data></Cell>
          <Cell ss:StyleID="s297a"><Data ss:Type="String"></Data></Cell>
          <Cell ss:StyleID="s297a"><Data ss:Type="String"></Data></Cell>
          <Cell ss:StyleID="s297a"><Data ss:Type="String"></Data></Cell>
        </Row>
      </xsl:when>
      <xsl:when test="object_address = 'Прочие' or object_address = 'Культура' or object_address = 'Образование' or object_address = 'Прочие' or object_address = 'Муниципальный фонд' or object_address = 'Ч/сектор'">
        <Row ss:Height="12.75" ss:StyleID="s293a">
          <Cell ss:StyleID="s294a"><Data ss:Type="String"><xsl:value-of select="number"/></Data></Cell>
          <Cell ss:StyleID="s295a"><Data ss:Type="String"><xsl:value-of select="object_address"/></Data></Cell>
          <Cell ss:StyleID="s295a"><Data ss:Type="String"><xsl:value-of select="region_name"/></Data></Cell>
          <Cell ss:StyleID="s295a"><Data ss:Type="String"><xsl:value-of select="municipal_name"/></Data></Cell>
          <Cell ss:StyleID="s295a"><Data ss:Type="String"><xsl:value-of select="city_name"/></Data></Cell>
          <Cell ss:StyleID="s295a"><Data ss:Type="String"><xsl:value-of select="boiler_name"/></Data></Cell>
          <Cell ss:StyleID="s295a"><Data ss:Type="String"><xsl:value-of select="boiler_num"/></Data></Cell>
          <Cell ss:StyleID="s294a"><Data ss:Type="Number"><xsl:value-of select="input_num"/></Data></Cell>
          <Cell ss:StyleID="s294a"><Data ss:Type="String"><xsl:value-of select="input_date"/></Data></Cell>
          <Cell ss:StyleID="s294a"><Data ss:Type="String"><xsl:value-of select="power_total"/></Data></Cell>
          <Cell ss:StyleID="s294a"><Data ss:Type="Number"><xsl:value-of select="power_kk_cw"/></Data></Cell>
          <Cell ss:StyleID="s295a"><Data ss:Type="Number"><xsl:value-of select="power_kk_kcw"/></Data></Cell>
          <Cell ss:StyleID="s295a"><Data ss:Type="Number"><xsl:value-of select="power_kk_how"/></Data></Cell>
          <Cell ss:StyleID="s295a"><Data ss:Type="Number"><xsl:value-of select="power_kk_to"/></Data></Cell>
          <Cell ss:StyleID="s295a"><Data ss:Type="Number"><xsl:value-of select="power_vk_cw"/></Data></Cell>
          <Cell ss:StyleID="s295a"><Data ss:Type="Number"><xsl:value-of select="power_vk_hcw"/></Data></Cell>
          <Cell ss:StyleID="s295a"><Data ss:Type="Number"><xsl:value-of select="power_vk_how"/></Data></Cell>
          <Cell ss:StyleID="s295a"><Data ss:Type="String"><xsl:value-of select="power_vk_to"/></Data></Cell>
          <Cell ss:StyleID="s295a"><Data ss:Type="Number"><xsl:value-of select="appr_ac"/></Data></Cell>
          <Cell ss:StyleID="s295a"><Data ss:Type="Number"><xsl:value-of select="appr_people_num"/></Data></Cell>
          <Cell ss:StyleID="s295a"><Data ss:Type="Number"><xsl:value-of select="appr_total_vol"/></Data></Cell>
          <Cell ss:StyleID="s295a"><Data ss:Type="Number"><xsl:value-of select="appr_vol_kk_cw"/></Data></Cell>
          <Cell ss:StyleID="s295a"><Data ss:Type="Number"><xsl:value-of select="appr_vol_kk_hcw"/></Data></Cell>
          <Cell ss:StyleID="s295a"><Data ss:Type="Number"><xsl:value-of select="appr_vol_kk_how"/></Data></Cell>
          <Cell ss:StyleID="s295a"><Data ss:Type="Number"><xsl:value-of select="appr_vol_kk_to"/></Data></Cell>
          
          <Cell ss:StyleID="s295a"><Data ss:Type="Number"><xsl:value-of select="appr_vol_vk_cw"/></Data></Cell>
          <Cell ss:StyleID="s295a"><Data ss:Type="Number"><xsl:value-of select="appr_vol_vk_hcw"/></Data></Cell>
          <Cell ss:StyleID="s295a"><Data ss:Type="Number"><xsl:value-of select="appr_vol_vk_how"/></Data></Cell>
          <Cell ss:StyleID="s295a"><Data ss:Type="Number"><xsl:value-of select="appr_vol_vk_to"/></Data></Cell>
          
          
          <Cell ss:StyleID="s295a"><Data ss:Type="String"><xsl:value-of select="prog_ac"/></Data></Cell>
          <Cell ss:StyleID="s295a"><Data ss:Type="Number"><xsl:value-of select="prog_people_num"/></Data></Cell>
          <Cell ss:StyleID="s295a"><Data ss:Type="Number"><xsl:value-of select="prog_total_val"/></Data></Cell>
          <Cell ss:StyleID="s295a"><Data ss:Type="Number"><xsl:value-of select="prog_vol_kk_cw"/></Data></Cell>
          <Cell ss:StyleID="s295a"><Data ss:Type="Number"><xsl:value-of select="prog_vol_kk_hcw"/></Data></Cell>
          <Cell ss:StyleID="s295a"><Data ss:Type="Number"><xsl:value-of select="prog_vol_kk_how"/></Data></Cell>
          <Cell ss:StyleID="s295a"><Data ss:Type="Number"><xsl:value-of select="prog_vol_kk_to"/></Data></Cell>
          <Cell ss:StyleID="s295a"><Data ss:Type="Number"><xsl:value-of select="prog_vol_vk_cw"/></Data></Cell>
          <Cell ss:StyleID="s295a"><Data ss:Type="Number"><xsl:value-of select="prog_vol_vk_hcw"/></Data></Cell>
          <Cell ss:StyleID="s295a"><Data ss:Type="Number"><xsl:value-of select="prog_vol_vk_how"/></Data></Cell>
          <Cell ss:StyleID="s295a"><Data ss:Type="Number"><xsl:value-of select="prog_vol_vk_to"/></Data></Cell>
          <Cell ss:StyleID="s295a"><Data ss:Type="String"></Data></Cell>
          <Cell ss:StyleID="s295a" ss:Formula="=RC[-14]-RC[-28]"><Data ss:Type="Number"></Data></Cell>
          <Cell ss:StyleID="s295a"><Data ss:Type="String"></Data></Cell>
          <Cell ss:StyleID="s295a"><Data ss:Type="String"></Data></Cell>
          <Cell ss:StyleID="s295a"><Data ss:Type="String"></Data></Cell>
          <Cell ss:StyleID="s295a"><Data ss:Type="String"></Data></Cell>
          
        </Row>
      </xsl:when>
      <xsl:when test="object_address != ''">
        <Row ss:Height="12.75" ss:StyleID="s300a">
          <Cell ss:StyleID="s302a"><Data ss:Type="String"><xsl:value-of select="number"/></Data></Cell>
          <Cell ss:StyleID="s302a"><Data ss:Type="String"><xsl:value-of select="object_address"/></Data></Cell>
          <Cell ss:StyleID="s302a"><Data ss:Type="String"><xsl:value-of select="region_name"/></Data></Cell>
          <Cell ss:StyleID="s302a"><Data ss:Type="String"><xsl:value-of select="municipal_name"/></Data></Cell>
          <Cell ss:StyleID="s302a"><Data ss:Type="String"><xsl:value-of select="city_name"/></Data></Cell>
          <Cell ss:StyleID="s302a"><Data ss:Type="String"><xsl:value-of select="boiler_name"/></Data></Cell>
          <Cell ss:StyleID="s302a"><Data ss:Type="String"><xsl:value-of select="boiler_num"/></Data></Cell>
          <Cell ss:StyleID="s302a"><Data ss:Type="Number"><xsl:value-of select="input_num"/></Data></Cell>
          <Cell ss:StyleID="s302a"><Data ss:Type="String"><xsl:value-of select="input_date"/></Data></Cell>
          <Cell ss:StyleID="s302a"><Data ss:Type="String"><xsl:value-of select="power_total"/></Data></Cell>
          <Cell ss:StyleID="s302a"><Data ss:Type="Number"><xsl:value-of select="power_kk_cw"/></Data></Cell>
          <Cell ss:StyleID="s302a"><Data ss:Type="Number"><xsl:value-of select="power_kk_kcw"/></Data></Cell>
          <Cell ss:StyleID="s302a"><Data ss:Type="Number"><xsl:value-of select="power_kk_how"/></Data></Cell>
          <Cell ss:StyleID="s302a"><Data ss:Type="Number"><xsl:value-of select="power_kk_to"/></Data></Cell>
          <Cell ss:StyleID="s302a"><Data ss:Type="Number"><xsl:value-of select="power_vk_cw"/></Data></Cell>
          <Cell ss:StyleID="s302a"><Data ss:Type="Number"><xsl:value-of select="power_vk_hcw"/></Data></Cell>
          <Cell ss:StyleID="s302a"><Data ss:Type="Number"><xsl:value-of select="power_vk_how"/></Data></Cell>
          <Cell ss:StyleID="s302a"><Data ss:Type="String"><xsl:value-of select="power_vk_to"/></Data></Cell>
          <Cell ss:StyleID="s302a"><Data ss:Type="Number"><xsl:value-of select="appr_ac"/></Data></Cell>
          <Cell ss:StyleID="s302a"><Data ss:Type="Number"><xsl:value-of select="appr_people_num"/></Data></Cell>
          <Cell ss:StyleID="s302a"><Data ss:Type="Number"><xsl:value-of select="appr_total_vol"/></Data></Cell>
          <Cell ss:StyleID="s302a"><Data ss:Type="Number"><xsl:value-of select="appr_vol_kk_cw"/></Data></Cell>
          <Cell ss:StyleID="s302a"><Data ss:Type="Number"><xsl:value-of select="appr_vol_kk_hcw"/></Data></Cell>
          <Cell ss:StyleID="s302a"><Data ss:Type="Number"><xsl:value-of select="appr_vol_kk_how"/></Data></Cell>
          <Cell ss:StyleID="s302a"><Data ss:Type="Number"><xsl:value-of select="appr_vol_kk_to"/></Data></Cell>
          
          <Cell ss:StyleID="s302a"><Data ss:Type="Number"><xsl:value-of select="appr_vol_vk_cw"/></Data></Cell>
          <Cell ss:StyleID="s302a"><Data ss:Type="Number"><xsl:value-of select="appr_vol_vk_hcw"/></Data></Cell>
          <Cell ss:StyleID="s302a"><Data ss:Type="Number"><xsl:value-of select="appr_vol_vk_how"/></Data></Cell>
          <Cell ss:StyleID="s302a"><Data ss:Type="Number"><xsl:value-of select="appr_vol_vk_to"/></Data></Cell>
          
          
          <Cell ss:StyleID="s302a"><Data ss:Type="String"><xsl:value-of select="prog_ac"/></Data></Cell>
          <Cell ss:StyleID="s302a"><Data ss:Type="Number"><xsl:value-of select="prog_people_num"/></Data></Cell>
          <Cell ss:StyleID="s302a"><Data ss:Type="Number"><xsl:value-of select="prog_total_val"/></Data></Cell>
          <Cell ss:StyleID="s302a"><Data ss:Type="Number"><xsl:value-of select="prog_vol_kk_cw"/></Data></Cell>
          <Cell ss:StyleID="s302a"><Data ss:Type="Number"><xsl:value-of select="prog_vol_kk_hcw"/></Data></Cell>
          <Cell ss:StyleID="s302a"><Data ss:Type="Number"><xsl:value-of select="prog_vol_kk_how"/></Data></Cell>
          <Cell ss:StyleID="s302a"><Data ss:Type="Number"><xsl:value-of select="prog_vol_kk_to"/></Data></Cell>
          <Cell ss:StyleID="s302a"><Data ss:Type="Number"><xsl:value-of select="prog_vol_vk_cw"/></Data></Cell>
          <Cell ss:StyleID="s302a"><Data ss:Type="Number"><xsl:value-of select="prog_vol_vk_hcw"/></Data></Cell>
          <Cell ss:StyleID="s302a"><Data ss:Type="Number"><xsl:value-of select="prog_vol_vk_how"/></Data></Cell>
          <Cell ss:StyleID="s302a"><Data ss:Type="Number"><xsl:value-of select="prog_vol_vk_to"/></Data></Cell>
          <Cell ss:StyleID="s302a"><Data ss:Type="String"></Data></Cell>
          <Cell ss:StyleID="s302a" ss:Formula="=RC[-14]-RC[-28]"><Data ss:Type="Number"></Data></Cell>
          <Cell ss:StyleID="s302a"><Data ss:Type="String"></Data></Cell>
          <Cell ss:StyleID="s302a"><Data ss:Type="String"></Data></Cell>
          <Cell ss:StyleID="s302a"><Data ss:Type="String"></Data></Cell>
          <Cell ss:StyleID="s302a"><Data ss:Type="String"></Data></Cell>
          
        </Row>
      </xsl:when>
    </xsl:choose>
  </xsl:template>
  
  
    <!-- <<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<< Шаблон данных листа ТХ31 >>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>> -->
  <xsl:template match="TS31">
    <xsl:choose>
      <xsl:when test="object_address = 'ВСЕГО:'">
        <Row ss:Height="12.75" ss:StyleID="s293a">
          <Cell ss:StyleID="s294a"><Data ss:Type="String"><xsl:value-of select="number"/></Data></Cell>
          <Cell ss:StyleID="s295a"><Data ss:Type="String"><xsl:value-of select="object_address"/></Data></Cell>
          <Cell ss:StyleID="s295a"><Data ss:Type="String"><xsl:value-of select="region_name"/></Data></Cell>
          <Cell ss:StyleID="s295a"><Data ss:Type="String"><xsl:value-of select="municipal_name"/></Data></Cell>
          <Cell ss:StyleID="s295a"><Data ss:Type="String"><xsl:value-of select="city_name"/></Data></Cell>
          <Cell ss:StyleID="s295a"><Data ss:Type="String"><xsl:value-of select="boiler_name"/></Data></Cell>
          <Cell ss:StyleID="s295a"><Data ss:Type="String"><xsl:value-of select="boiler_num"/></Data></Cell>
          <Cell ss:StyleID="s294a"><Data ss:Type="Number"><xsl:value-of select="input_num"/></Data></Cell>
          <Cell ss:StyleID="s294a"><Data ss:Type="String"><xsl:value-of select="input_date"/></Data></Cell>
          <Cell ss:StyleID="s294a"><Data ss:Type="String"><xsl:value-of select="power_total"/></Data></Cell>
          <Cell ss:StyleID="s294a"><Data ss:Type="Number"><xsl:value-of select="power_kk_cw"/></Data></Cell>
          <Cell ss:StyleID="s295a"><Data ss:Type="Number"><xsl:value-of select="power_kk_kcw"/></Data></Cell>
          <Cell ss:StyleID="s295a"><Data ss:Type="Number"><xsl:value-of select="power_kk_how"/></Data></Cell>
          <Cell ss:StyleID="s295a"><Data ss:Type="Number"><xsl:value-of select="power_kk_to"/></Data></Cell>
          <Cell ss:StyleID="s295a"><Data ss:Type="Number"><xsl:value-of select="power_vk_cw"/></Data></Cell>
          <Cell ss:StyleID="s295a"><Data ss:Type="Number"><xsl:value-of select="power_vk_hcw"/></Data></Cell>
          <Cell ss:StyleID="s295a"><Data ss:Type="Number"><xsl:value-of select="power_vk_how"/></Data></Cell>
          <Cell ss:StyleID="s295a"><Data ss:Type="String"><xsl:value-of select="power_vk_to"/></Data></Cell>
          <Cell ss:StyleID="s295a"><Data ss:Type="Number"><xsl:value-of select="appr_ac"/></Data></Cell>
          <Cell ss:StyleID="s295a"><Data ss:Type="Number"><xsl:value-of select="appr_people_num"/></Data></Cell>
          <Cell ss:StyleID="s295a"><Data ss:Type="Number"><xsl:value-of select="appr_total_vol"/></Data></Cell>
          <Cell ss:StyleID="s295a"><Data ss:Type="Number"><xsl:value-of select="appr_vol_kk_cw"/></Data></Cell>
          <Cell ss:StyleID="s295a"><Data ss:Type="Number"><xsl:value-of select="appr_vol_kk_hcw"/></Data></Cell>
          <Cell ss:StyleID="s295a"><Data ss:Type="Number"><xsl:value-of select="appr_vol_kk_how"/></Data></Cell>
          <Cell ss:StyleID="s295a"><Data ss:Type="Number"><xsl:value-of select="appr_vol_kk_to"/></Data></Cell>
          
          <Cell ss:StyleID="s295a"><Data ss:Type="Number"><xsl:value-of select="appr_vol_vk_cw"/></Data></Cell>
          <Cell ss:StyleID="s295a"><Data ss:Type="Number"><xsl:value-of select="appr_vol_vk_hcw"/></Data></Cell>
          <Cell ss:StyleID="s295a"><Data ss:Type="Number"><xsl:value-of select="appr_vol_vk_how"/></Data></Cell>
          <Cell ss:StyleID="s295a"><Data ss:Type="Number"><xsl:value-of select="appr_vol_vk_to"/></Data></Cell>
          
          
          <Cell ss:StyleID="s295a"><Data ss:Type="String"><xsl:value-of select="prog_ac"/></Data></Cell>
          <Cell ss:StyleID="s295a"><Data ss:Type="Number"><xsl:value-of select="prog_people_num"/></Data></Cell>
          <Cell ss:StyleID="s295a"><Data ss:Type="Number"><xsl:value-of select="prog_total_val"/></Data></Cell>
          <Cell ss:StyleID="s295a"><Data ss:Type="Number"><xsl:value-of select="prog_vol_kk_cw"/></Data></Cell>
          <Cell ss:StyleID="s295a"><Data ss:Type="Number"><xsl:value-of select="prog_vol_kk_hcw"/></Data></Cell>
          <Cell ss:StyleID="s295a"><Data ss:Type="Number"><xsl:value-of select="prog_vol_kk_how"/></Data></Cell>
          <Cell ss:StyleID="s295a"><Data ss:Type="Number"><xsl:value-of select="prog_vol_kk_to"/></Data></Cell>
          <Cell ss:StyleID="s295a"><Data ss:Type="Number"><xsl:value-of select="prog_vol_vk_cw"/></Data></Cell>
          <Cell ss:StyleID="s295a"><Data ss:Type="Number"><xsl:value-of select="prog_vol_vk_hcw"/></Data></Cell>
          <Cell ss:StyleID="s295a"><Data ss:Type="Number"><xsl:value-of select="prog_vol_vk_how"/></Data></Cell>
          <Cell ss:StyleID="s295a"><Data ss:Type="Number"><xsl:value-of select="prog_vol_vk_to"/></Data></Cell>
          <Cell ss:StyleID="s295a"><Data ss:Type="String"></Data></Cell>
          <Cell ss:StyleID="s295a" ss:Formula="=RC[-14]-RC[-28]"><Data ss:Type="Number"></Data></Cell>
          <Cell ss:StyleID="s295a"><Data ss:Type="String"></Data></Cell>
          <Cell ss:StyleID="s295a"><Data ss:Type="String"></Data></Cell>
          <Cell ss:StyleID="s295a"><Data ss:Type="String"></Data></Cell>
          <Cell ss:StyleID="s295a"><Data ss:Type="String"></Data></Cell>
        </Row>
      </xsl:when>
      <xsl:when test="number = 1 or number = 2 or number = 3 or number = 4 or number = 5 or number = 6">
        <Row ss:Height="12.75" ss:StyleID="s293a">
          <Cell ss:StyleID="s297a"><Data ss:Type="String"><xsl:value-of select="number"/></Data></Cell>
          <Cell ss:StyleID="s297a"><Data ss:Type="String"><xsl:value-of select="object_address"/></Data></Cell>
          <Cell ss:StyleID="s297a"><Data ss:Type="String"><xsl:value-of select="region_name"/></Data></Cell>
          <Cell ss:StyleID="s297a"><Data ss:Type="String"><xsl:value-of select="municipal_name"/></Data></Cell>
          <Cell ss:StyleID="s297a"><Data ss:Type="String"><xsl:value-of select="city_name"/></Data></Cell>
          <Cell ss:StyleID="s297a"><Data ss:Type="String"><xsl:value-of select="boiler_name"/></Data></Cell>
          <Cell ss:StyleID="s297a"><Data ss:Type="String"><xsl:value-of select="boiler_num"/></Data></Cell>
          <Cell ss:StyleID="s297a"><Data ss:Type="Number"><xsl:value-of select="input_num"/></Data></Cell>
          <Cell ss:StyleID="s297a"><Data ss:Type="String"><xsl:value-of select="input_date"/></Data></Cell>
          <Cell ss:StyleID="s297a"><Data ss:Type="String"><xsl:value-of select="power_total"/></Data></Cell>
          <Cell ss:StyleID="s297a"><Data ss:Type="Number"><xsl:value-of select="power_kk_cw"/></Data></Cell>
          <Cell ss:StyleID="s297a"><Data ss:Type="Number"><xsl:value-of select="power_kk_kcw"/></Data></Cell>
          <Cell ss:StyleID="s297a"><Data ss:Type="Number"><xsl:value-of select="power_kk_how"/></Data></Cell>
          <Cell ss:StyleID="s297a"><Data ss:Type="Number"><xsl:value-of select="power_kk_to"/></Data></Cell>
          <Cell ss:StyleID="s297a"><Data ss:Type="Number"><xsl:value-of select="power_vk_cw"/></Data></Cell>
          <Cell ss:StyleID="s297a"><Data ss:Type="Number"><xsl:value-of select="power_vk_hcw"/></Data></Cell>
          <Cell ss:StyleID="s297a"><Data ss:Type="Number"><xsl:value-of select="power_vk_how"/></Data></Cell>
          <Cell ss:StyleID="s297a"><Data ss:Type="String"><xsl:value-of select="power_vk_to"/></Data></Cell>
          <Cell ss:StyleID="s297a"><Data ss:Type="Number"><xsl:value-of select="appr_ac"/></Data></Cell>
          <Cell ss:StyleID="s297a"><Data ss:Type="Number"><xsl:value-of select="appr_people_num"/></Data></Cell>
          <Cell ss:StyleID="s297a"><Data ss:Type="Number"><xsl:value-of select="appr_total_vol"/></Data></Cell>
          <Cell ss:StyleID="s297a"><Data ss:Type="Number"><xsl:value-of select="appr_vol_kk_cw"/></Data></Cell>
          <Cell ss:StyleID="s297a"><Data ss:Type="Number"><xsl:value-of select="appr_vol_kk_hcw"/></Data></Cell>
          <Cell ss:StyleID="s297a"><Data ss:Type="Number"><xsl:value-of select="appr_vol_kk_how"/></Data></Cell>
          <Cell ss:StyleID="s297a"><Data ss:Type="Number"><xsl:value-of select="appr_vol_kk_to"/></Data></Cell>
          
          <Cell ss:StyleID="s297a"><Data ss:Type="Number"><xsl:value-of select="appr_vol_vk_cw"/></Data></Cell>
          <Cell ss:StyleID="s297a"><Data ss:Type="Number"><xsl:value-of select="appr_vol_vk_hcw"/></Data></Cell>
          <Cell ss:StyleID="s297a"><Data ss:Type="Number"><xsl:value-of select="appr_vol_vk_how"/></Data></Cell>
          <Cell ss:StyleID="s297a"><Data ss:Type="Number"><xsl:value-of select="appr_vol_vk_to"/></Data></Cell>
          
          
          <Cell ss:StyleID="s297a"><Data ss:Type="String"><xsl:value-of select="prog_ac"/></Data></Cell>
          <Cell ss:StyleID="s297a"><Data ss:Type="Number"><xsl:value-of select="prog_people_num"/></Data></Cell>
          <Cell ss:StyleID="s297a"><Data ss:Type="Number"><xsl:value-of select="prog_total_val"/></Data></Cell>
          <Cell ss:StyleID="s297a"><Data ss:Type="Number"><xsl:value-of select="prog_vol_kk_cw"/></Data></Cell>
          <Cell ss:StyleID="s297a"><Data ss:Type="Number"><xsl:value-of select="prog_vol_kk_hcw"/></Data></Cell>
          <Cell ss:StyleID="s297a"><Data ss:Type="Number"><xsl:value-of select="prog_vol_kk_how"/></Data></Cell>
          <Cell ss:StyleID="s297a"><Data ss:Type="Number"><xsl:value-of select="prog_vol_kk_to"/></Data></Cell>
          <Cell ss:StyleID="s297a"><Data ss:Type="Number"><xsl:value-of select="prog_vol_vk_cw"/></Data></Cell>
          <Cell ss:StyleID="s297a"><Data ss:Type="Number"><xsl:value-of select="prog_vol_vk_hcw"/></Data></Cell>
          <Cell ss:StyleID="s297a"><Data ss:Type="Number"><xsl:value-of select="prog_vol_vk_how"/></Data></Cell>
          <Cell ss:StyleID="s297a"><Data ss:Type="Number"><xsl:value-of select="prog_vol_vk_to"/></Data></Cell>
          <Cell ss:StyleID="s297a"><Data ss:Type="String"></Data></Cell>
          <Cell ss:StyleID="s297a" ss:Formula="=RC[-14]-RC[-28]"><Data ss:Type="Number"></Data></Cell>
          <Cell ss:StyleID="s297a"><Data ss:Type="String"></Data></Cell>
          <Cell ss:StyleID="s297a"><Data ss:Type="String"></Data></Cell>
          <Cell ss:StyleID="s297a"><Data ss:Type="String"></Data></Cell>
          <Cell ss:StyleID="s297a"><Data ss:Type="String"></Data></Cell>
        </Row>
      </xsl:when>
      <xsl:when test="object_address = 'Прочие' or object_address = 'Культура' or object_address = 'Образование' or object_address = 'Прочие' or object_address = 'Муниципальный фонд' or object_address = 'Ч/сектор'">
        <Row ss:Height="12.75" ss:StyleID="s293a">
          <Cell ss:StyleID="s294a"><Data ss:Type="String"><xsl:value-of select="number"/></Data></Cell>
          <Cell ss:StyleID="s295a"><Data ss:Type="String"><xsl:value-of select="object_address"/></Data></Cell>
          <Cell ss:StyleID="s295a"><Data ss:Type="String"><xsl:value-of select="region_name"/></Data></Cell>
          <Cell ss:StyleID="s295a"><Data ss:Type="String"><xsl:value-of select="municipal_name"/></Data></Cell>
          <Cell ss:StyleID="s295a"><Data ss:Type="String"><xsl:value-of select="city_name"/></Data></Cell>
          <Cell ss:StyleID="s295a"><Data ss:Type="String"><xsl:value-of select="boiler_name"/></Data></Cell>
          <Cell ss:StyleID="s295a"><Data ss:Type="String"><xsl:value-of select="boiler_num"/></Data></Cell>
          <Cell ss:StyleID="s294a"><Data ss:Type="Number"><xsl:value-of select="input_num"/></Data></Cell>
          <Cell ss:StyleID="s294a"><Data ss:Type="String"><xsl:value-of select="input_date"/></Data></Cell>
          <Cell ss:StyleID="s294a"><Data ss:Type="String"><xsl:value-of select="power_total"/></Data></Cell>
          <Cell ss:StyleID="s294a"><Data ss:Type="Number"><xsl:value-of select="power_kk_cw"/></Data></Cell>
          <Cell ss:StyleID="s295a"><Data ss:Type="Number"><xsl:value-of select="power_kk_kcw"/></Data></Cell>
          <Cell ss:StyleID="s295a"><Data ss:Type="Number"><xsl:value-of select="power_kk_how"/></Data></Cell>
          <Cell ss:StyleID="s295a"><Data ss:Type="Number"><xsl:value-of select="power_kk_to"/></Data></Cell>
          <Cell ss:StyleID="s295a"><Data ss:Type="Number"><xsl:value-of select="power_vk_cw"/></Data></Cell>
          <Cell ss:StyleID="s295a"><Data ss:Type="Number"><xsl:value-of select="power_vk_hcw"/></Data></Cell>
          <Cell ss:StyleID="s295a"><Data ss:Type="Number"><xsl:value-of select="power_vk_how"/></Data></Cell>
          <Cell ss:StyleID="s295a"><Data ss:Type="String"><xsl:value-of select="power_vk_to"/></Data></Cell>
          <Cell ss:StyleID="s295a"><Data ss:Type="Number"><xsl:value-of select="appr_ac"/></Data></Cell>
          <Cell ss:StyleID="s295a"><Data ss:Type="Number"><xsl:value-of select="appr_people_num"/></Data></Cell>
          <Cell ss:StyleID="s295a"><Data ss:Type="Number"><xsl:value-of select="appr_total_vol"/></Data></Cell>
          <Cell ss:StyleID="s295a"><Data ss:Type="Number"><xsl:value-of select="appr_vol_kk_cw"/></Data></Cell>
          <Cell ss:StyleID="s295a"><Data ss:Type="Number"><xsl:value-of select="appr_vol_kk_hcw"/></Data></Cell>
          <Cell ss:StyleID="s295a"><Data ss:Type="Number"><xsl:value-of select="appr_vol_kk_how"/></Data></Cell>
          <Cell ss:StyleID="s295a"><Data ss:Type="Number"><xsl:value-of select="appr_vol_kk_to"/></Data></Cell>
          
          <Cell ss:StyleID="s295a"><Data ss:Type="Number"><xsl:value-of select="appr_vol_vk_cw"/></Data></Cell>
          <Cell ss:StyleID="s295a"><Data ss:Type="Number"><xsl:value-of select="appr_vol_vk_hcw"/></Data></Cell>
          <Cell ss:StyleID="s295a"><Data ss:Type="Number"><xsl:value-of select="appr_vol_vk_how"/></Data></Cell>
          <Cell ss:StyleID="s295a"><Data ss:Type="Number"><xsl:value-of select="appr_vol_vk_to"/></Data></Cell>
          
          
          <Cell ss:StyleID="s295a"><Data ss:Type="String"><xsl:value-of select="prog_ac"/></Data></Cell>
          <Cell ss:StyleID="s295a"><Data ss:Type="Number"><xsl:value-of select="prog_people_num"/></Data></Cell>
          <Cell ss:StyleID="s295a"><Data ss:Type="Number"><xsl:value-of select="prog_total_val"/></Data></Cell>
          <Cell ss:StyleID="s295a"><Data ss:Type="Number"><xsl:value-of select="prog_vol_kk_cw"/></Data></Cell>
          <Cell ss:StyleID="s295a"><Data ss:Type="Number"><xsl:value-of select="prog_vol_kk_hcw"/></Data></Cell>
          <Cell ss:StyleID="s295a"><Data ss:Type="Number"><xsl:value-of select="prog_vol_kk_how"/></Data></Cell>
          <Cell ss:StyleID="s295a"><Data ss:Type="Number"><xsl:value-of select="prog_vol_kk_to"/></Data></Cell>
          <Cell ss:StyleID="s295a"><Data ss:Type="Number"><xsl:value-of select="prog_vol_vk_cw"/></Data></Cell>
          <Cell ss:StyleID="s295a"><Data ss:Type="Number"><xsl:value-of select="prog_vol_vk_hcw"/></Data></Cell>
          <Cell ss:StyleID="s295a"><Data ss:Type="Number"><xsl:value-of select="prog_vol_vk_how"/></Data></Cell>
          <Cell ss:StyleID="s295a"><Data ss:Type="Number"><xsl:value-of select="prog_vol_vk_to"/></Data></Cell>
          <Cell ss:StyleID="s295a"><Data ss:Type="String"></Data></Cell>
          <Cell ss:StyleID="s295a" ss:Formula="=RC[-14]-RC[-28]"><Data ss:Type="Number"></Data></Cell>
          <Cell ss:StyleID="s295a"><Data ss:Type="String"></Data></Cell>
          <Cell ss:StyleID="s295a"><Data ss:Type="String"></Data></Cell>
          <Cell ss:StyleID="s295a"><Data ss:Type="String"></Data></Cell>
          <Cell ss:StyleID="s295a"><Data ss:Type="String"></Data></Cell>
          
        </Row>
      </xsl:when>
      <xsl:when test="object_address != ''">
        <Row ss:Height="12.75" ss:StyleID="s300a">
          <Cell ss:StyleID="s302a"><Data ss:Type="String"><xsl:value-of select="number"/></Data></Cell>
          <Cell ss:StyleID="s302a"><Data ss:Type="String"><xsl:value-of select="object_address"/></Data></Cell>
          <Cell ss:StyleID="s302a"><Data ss:Type="String"><xsl:value-of select="region_name"/></Data></Cell>
          <Cell ss:StyleID="s302a"><Data ss:Type="String"><xsl:value-of select="municipal_name"/></Data></Cell>
          <Cell ss:StyleID="s302a"><Data ss:Type="String"><xsl:value-of select="city_name"/></Data></Cell>
          <Cell ss:StyleID="s302a"><Data ss:Type="String"><xsl:value-of select="boiler_name"/></Data></Cell>
          <Cell ss:StyleID="s302a"><Data ss:Type="String"><xsl:value-of select="boiler_num"/></Data></Cell>
          <Cell ss:StyleID="s302a"><Data ss:Type="Number"><xsl:value-of select="input_num"/></Data></Cell>
          <Cell ss:StyleID="s302a"><Data ss:Type="String"><xsl:value-of select="input_date"/></Data></Cell>
          <Cell ss:StyleID="s302a"><Data ss:Type="String"><xsl:value-of select="power_total"/></Data></Cell>
          <Cell ss:StyleID="s302a"><Data ss:Type="Number"><xsl:value-of select="power_kk_cw"/></Data></Cell>
          <Cell ss:StyleID="s302a"><Data ss:Type="Number"><xsl:value-of select="power_kk_kcw"/></Data></Cell>
          <Cell ss:StyleID="s302a"><Data ss:Type="Number"><xsl:value-of select="power_kk_how"/></Data></Cell>
          <Cell ss:StyleID="s302a"><Data ss:Type="Number"><xsl:value-of select="power_kk_to"/></Data></Cell>
          <Cell ss:StyleID="s302a"><Data ss:Type="Number"><xsl:value-of select="power_vk_cw"/></Data></Cell>
          <Cell ss:StyleID="s302a"><Data ss:Type="Number"><xsl:value-of select="power_vk_hcw"/></Data></Cell>
          <Cell ss:StyleID="s302a"><Data ss:Type="Number"><xsl:value-of select="power_vk_how"/></Data></Cell>
          <Cell ss:StyleID="s302a"><Data ss:Type="String"><xsl:value-of select="power_vk_to"/></Data></Cell>
          <Cell ss:StyleID="s302a"><Data ss:Type="Number"><xsl:value-of select="appr_ac"/></Data></Cell>
          <Cell ss:StyleID="s302a"><Data ss:Type="Number"><xsl:value-of select="appr_people_num"/></Data></Cell>
          <Cell ss:StyleID="s302a"><Data ss:Type="Number"><xsl:value-of select="appr_total_vol"/></Data></Cell>
          <Cell ss:StyleID="s302a"><Data ss:Type="Number"><xsl:value-of select="appr_vol_kk_cw"/></Data></Cell>
          <Cell ss:StyleID="s302a"><Data ss:Type="Number"><xsl:value-of select="appr_vol_kk_hcw"/></Data></Cell>
          <Cell ss:StyleID="s302a"><Data ss:Type="Number"><xsl:value-of select="appr_vol_kk_how"/></Data></Cell>
          <Cell ss:StyleID="s302a"><Data ss:Type="Number"><xsl:value-of select="appr_vol_kk_to"/></Data></Cell>
          
          <Cell ss:StyleID="s302a"><Data ss:Type="Number"><xsl:value-of select="appr_vol_vk_cw"/></Data></Cell>
          <Cell ss:StyleID="s302a"><Data ss:Type="Number"><xsl:value-of select="appr_vol_vk_hcw"/></Data></Cell>
          <Cell ss:StyleID="s302a"><Data ss:Type="Number"><xsl:value-of select="appr_vol_vk_how"/></Data></Cell>
          <Cell ss:StyleID="s302a"><Data ss:Type="Number"><xsl:value-of select="appr_vol_vk_to"/></Data></Cell>
          
          
          <Cell ss:StyleID="s302a"><Data ss:Type="String"><xsl:value-of select="prog_ac"/></Data></Cell>
          <Cell ss:StyleID="s302a"><Data ss:Type="Number"><xsl:value-of select="prog_people_num"/></Data></Cell>
          <Cell ss:StyleID="s302a"><Data ss:Type="Number"><xsl:value-of select="prog_total_val"/></Data></Cell>
          <Cell ss:StyleID="s302a"><Data ss:Type="Number"><xsl:value-of select="prog_vol_kk_cw"/></Data></Cell>
          <Cell ss:StyleID="s302a"><Data ss:Type="Number"><xsl:value-of select="prog_vol_kk_hcw"/></Data></Cell>
          <Cell ss:StyleID="s302a"><Data ss:Type="Number"><xsl:value-of select="prog_vol_kk_how"/></Data></Cell>
          <Cell ss:StyleID="s302a"><Data ss:Type="Number"><xsl:value-of select="prog_vol_kk_to"/></Data></Cell>
          <Cell ss:StyleID="s302a"><Data ss:Type="Number"><xsl:value-of select="prog_vol_vk_cw"/></Data></Cell>
          <Cell ss:StyleID="s302a"><Data ss:Type="Number"><xsl:value-of select="prog_vol_vk_hcw"/></Data></Cell>
          <Cell ss:StyleID="s302a"><Data ss:Type="Number"><xsl:value-of select="prog_vol_vk_how"/></Data></Cell>
          <Cell ss:StyleID="s302a"><Data ss:Type="Number"><xsl:value-of select="prog_vol_vk_to"/></Data></Cell>
          <Cell ss:StyleID="s302a"><Data ss:Type="String"></Data></Cell>
          <Cell ss:StyleID="s302a" ss:Formula="=RC[-14]-RC[-28]"><Data ss:Type="Number"></Data></Cell>
          <Cell ss:StyleID="s302a"><Data ss:Type="String"></Data></Cell>
          <Cell ss:StyleID="s302a"><Data ss:Type="String"></Data></Cell>
          <Cell ss:StyleID="s302a"><Data ss:Type="String"></Data></Cell>
          <Cell ss:StyleID="s302a"><Data ss:Type="String"></Data></Cell>
          
        </Row>
      </xsl:when>
    </xsl:choose>
  </xsl:template>
      
  <!-- <<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<< Шаблон данных листа ТХ1 >>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>> -->
  <xsl:template match="TS1">
    <xsl:choose>
      <xsl:when test="object_address = 'ВСЕГО:'">
        <Row ss:Height="12.75" ss:StyleID="s293a">
          <Cell ss:StyleID="s294a"><Data ss:Type="String"><xsl:value-of select="number"/></Data></Cell>
          <Cell ss:StyleID="s295a"><Data ss:Type="String"><xsl:value-of select="object_address"/></Data></Cell>
          <Cell ss:StyleID="s295a"><Data ss:Type="String"><xsl:value-of select="region_name"/></Data></Cell>
          <Cell ss:StyleID="s295a"><Data ss:Type="String"><xsl:value-of select="municipal_name"/></Data></Cell>
          <Cell ss:StyleID="s295a"><Data ss:Type="String"><xsl:value-of select="city_name"/></Data></Cell>
          <Cell ss:StyleID="s295a"><Data ss:Type="String"><xsl:value-of select="boiler_name"/></Data></Cell>
          <Cell ss:StyleID="s295a"><Data ss:Type="String"><xsl:value-of select="boiler_num"/></Data></Cell>
          <Cell ss:StyleID="s294a"><Data ss:Type="Number"><xsl:value-of select="input_num"/></Data></Cell>
          <Cell ss:StyleID="s294a"><Data ss:Type="String"><xsl:value-of select="input_date"/></Data></Cell>
          <Cell ss:StyleID="s294a"><Data ss:Type="String"><xsl:value-of select="ac"/></Data></Cell>
          <Cell ss:StyleID="s294a"><Data ss:Type="Number"><xsl:value-of select="heating_period"/></Data></Cell>
          <Cell ss:StyleID="s295a"><Data ss:Type="Number"><xsl:value-of select="power_total"/></Data></Cell>
          <Cell ss:StyleID="s295a"><Data ss:Type="Number"><xsl:value-of select="power_heat"/></Data></Cell>
          <Cell ss:StyleID="s295a"><Data ss:Type="Number"><xsl:value-of select="power_heat_cw"/></Data></Cell>
          <Cell ss:StyleID="s295a"><Data ss:Type="Number"><xsl:value-of select="power_heat_ow"/></Data></Cell>
          <Cell ss:StyleID="s295a"><Data ss:Type="Number"><xsl:value-of select="appr_floor_num"/></Data></Cell>
          <Cell ss:StyleID="s295a"><Data ss:Type="String"><xsl:value-of select="appr_ac"/></Data></Cell>
          <Cell ss:StyleID="s295a"><Data ss:Type="Number"><xsl:value-of select="appr_people_num"/></Data></Cell>
          <Cell ss:StyleID="s295a"><Data ss:Type="Number"><xsl:value-of select="appr_db_glass"/></Data></Cell>
          <Cell ss:StyleID="s295a"><Data ss:Type="Number"><xsl:value-of select="appr_plp"/></Data></Cell>
          <Cell ss:StyleID="s295a"><Data ss:Type="Number"><xsl:value-of select="appr_object_volume"/></Data></Cell>
          <Cell ss:StyleID="s295a"><Data ss:Type="Number"><xsl:value-of select="appr_total_vol"/></Data></Cell>
          <Cell ss:StyleID="s295a"><Data ss:Type="Number"><xsl:value-of select="appr_vol_heat"/></Data></Cell>
          <Cell ss:StyleID="s295a"><Data ss:Type="Number"><xsl:value-of select="appr_vol_vent"/></Data></Cell>
          <Cell ss:StyleID="s295a"><Data ss:Type="Number"><xsl:value-of select="appr_vol_sput"/></Data></Cell>
          <Cell ss:StyleID="s295a"><Data ss:Type="Number"><xsl:value-of select="appr_vol_lost"/></Data></Cell>
          <Cell ss:StyleID="s295a"><Data ss:Type="Number"><xsl:value-of select="appr_vol_heat_cw"/></Data></Cell>
          <Cell ss:StyleID="s295a"><Data ss:Type="Number"><xsl:value-of select="appr_vol_heat_ow"/></Data></Cell>
          <Cell ss:StyleID="s295a"><Data ss:Type="Number"><xsl:value-of select="appr_vol_to"/></Data></Cell>
          <Cell ss:StyleID="s295a"><Data ss:Type="Number"><xsl:value-of select="prog_floor_num"/></Data></Cell>
          <Cell ss:StyleID="s295a"><Data ss:Type="String"><xsl:value-of select="prog_ac"/></Data></Cell>
          <Cell ss:StyleID="s295a"><Data ss:Type="Number"><xsl:value-of select="prog_people_num"/></Data></Cell>
          <Cell ss:StyleID="s295a"><Data ss:Type="Number"><xsl:value-of select="prog_db_glass"/></Data></Cell>
          <Cell ss:StyleID="s295a"><Data ss:Type="Number"><xsl:value-of select="prog_plp"/></Data></Cell>
          <Cell ss:StyleID="s295a"><Data ss:Type="Number"><xsl:value-of select="prog_object_volume"/></Data></Cell>
          <Cell ss:StyleID="s295a"><Data ss:Type="Number"><xsl:value-of select="prog_total_vol"/></Data></Cell>
          <Cell ss:StyleID="s295a"><Data ss:Type="Number"><xsl:value-of select="prog_vol_heat"/></Data></Cell>
          <Cell ss:StyleID="s295a"><Data ss:Type="Number"><xsl:value-of select="prog_vol_vent"/></Data></Cell>
          <Cell ss:StyleID="s295a"><Data ss:Type="Number"><xsl:value-of select="prog_vol_sput"/></Data></Cell>
          <Cell ss:StyleID="s295a"><Data ss:Type="Number"><xsl:value-of select="prog_vol_lost"/></Data></Cell>
          <Cell ss:StyleID="s295a"><Data ss:Type="Number"><xsl:value-of select="prog_vol_heat_cw"/></Data></Cell>
          <Cell ss:StyleID="s295a"><Data ss:Type="Number"><xsl:value-of select="prog_vol_heat_ow"/></Data></Cell>
          <Cell ss:StyleID="s295a"><Data ss:Type="Number"><xsl:value-of select="prog_vol_to"/></Data></Cell>
          <Cell ss:StyleID="s295a" ss:Formula="=RC[-14]-RC[-28]"><Data ss:Type="Number"></Data></Cell>
          <Cell ss:StyleID="s295a" ss:Formula="=RC[-14]-RC[-28]"><Data ss:Type="Number"></Data></Cell>
          <Cell ss:StyleID="s295a" ss:Formula="=RC[-14]-RC[-28]"><Data ss:Type="Number"></Data></Cell>
          <Cell ss:StyleID="s295a" ss:Formula="=RC[-14]-RC[-28]"><Data ss:Type="Number"></Data></Cell>
          <Cell ss:StyleID="s295a" ss:Formula="=RC[-14]-RC[-28]"><Data ss:Type="Number"></Data></Cell>
          <Cell ss:StyleID="s295a" ss:Formula="=RC[-14]-RC[-28]"><Data ss:Type="Number"></Data></Cell>
          <Cell ss:StyleID="s295a" ss:Formula="=RC[-14]-RC[-28]"><Data ss:Type="Number"></Data></Cell>
          <Cell ss:StyleID="s295a" ss:Formula="=RC[-14]-RC[-28]"><Data ss:Type="Number"></Data></Cell>
          <Cell ss:StyleID="s295a" ss:Formula="=RC[-14]-RC[-28]"><Data ss:Type="Number"></Data></Cell>
          <Cell ss:StyleID="s295a" ss:Formula="=RC[-14]-RC[-28]"><Data ss:Type="Number"></Data></Cell>
          <Cell ss:StyleID="s295a" ss:Formula="=RC[-14]-RC[-28]"><Data ss:Type="Number"></Data></Cell>
          <Cell ss:StyleID="s295a" ss:Formula="=RC[-14]-RC[-28]"><Data ss:Type="Number"></Data></Cell>
          <Cell ss:StyleID="s295a" ss:Formula="=RC[-14]-RC[-28]"><Data ss:Type="Number"></Data></Cell>
          <Cell ss:StyleID="s295a" ss:Formula="=RC[-14]-RC[-28]"><Data ss:Type="Number"></Data></Cell>
          <Cell ss:StyleID="s295a"><Data ss:Type="String"></Data></Cell>
        </Row>
      </xsl:when>
      <xsl:when test="number = 1 or number = 2 or number = 3 or number = 4 or number = 5 or number = 6">
        <Row ss:Height="12.75" ss:StyleID="s293a">
          <Cell ss:StyleID="s296a"><Data ss:Type="String"><xsl:value-of select="number"/></Data></Cell>
          <Cell ss:StyleID="s297a"><Data ss:Type="String"><xsl:value-of select="object_address"/></Data></Cell>
          <Cell ss:StyleID="s297a"><Data ss:Type="String"><xsl:value-of select="region_name"/></Data></Cell>
          <Cell ss:StyleID="s297a"><Data ss:Type="String"><xsl:value-of select="municipal_name"/></Data></Cell>
          <Cell ss:StyleID="s297a"><Data ss:Type="String"><xsl:value-of select="city_name"/></Data></Cell>
          <Cell ss:StyleID="s297a"><Data ss:Type="String"><xsl:value-of select="boiler_name"/></Data></Cell>
          <Cell ss:StyleID="s297a"><Data ss:Type="String"><xsl:value-of select="boiler_num"/></Data></Cell>
          <Cell ss:StyleID="s296a"><Data ss:Type="Number"><xsl:value-of select="input_num"/></Data></Cell>
          <Cell ss:StyleID="s296a"><Data ss:Type="String"><xsl:value-of select="input_date"/></Data></Cell>
          <Cell ss:StyleID="s296a"><Data ss:Type="String"><xsl:value-of select="ac"/></Data></Cell>
          <Cell ss:StyleID="s296a"><Data ss:Type="Number"><xsl:value-of select="heating_period"/></Data></Cell>
          <Cell ss:StyleID="s297a"><Data ss:Type="Number"><xsl:value-of select="power_total"/></Data></Cell>
          <Cell ss:StyleID="s297a"><Data ss:Type="Number"><xsl:value-of select="power_heat"/></Data></Cell>
          <Cell ss:StyleID="s297a"><Data ss:Type="Number"><xsl:value-of select="power_heat_cw"/></Data></Cell>
          <Cell ss:StyleID="s297a"><Data ss:Type="Number"><xsl:value-of select="power_heat_ow"/></Data></Cell>
          <Cell ss:StyleID="s297a"><Data ss:Type="Number"><xsl:value-of select="appr_floor_num"/></Data></Cell>
          <Cell ss:StyleID="s297a"><Data ss:Type="String"><xsl:value-of select="appr_ac"/></Data></Cell>
          <Cell ss:StyleID="s297a"><Data ss:Type="Number"><xsl:value-of select="appr_people_num"/></Data></Cell>
          <Cell ss:StyleID="s297a"><Data ss:Type="Number"><xsl:value-of select="appr_db_glass"/></Data></Cell>
          <Cell ss:StyleID="s297a"><Data ss:Type="Number"><xsl:value-of select="appr_plp"/></Data></Cell>
          <Cell ss:StyleID="s297a"><Data ss:Type="Number"><xsl:value-of select="appr_object_volume"/></Data></Cell>
          <Cell ss:StyleID="s297a"><Data ss:Type="Number"><xsl:value-of select="appr_total_vol"/></Data></Cell>
          <Cell ss:StyleID="s297a"><Data ss:Type="Number"><xsl:value-of select="appr_vol_heat"/></Data></Cell>
          <Cell ss:StyleID="s297a"><Data ss:Type="Number"><xsl:value-of select="appr_vol_vent"/></Data></Cell>
          <Cell ss:StyleID="s297a"><Data ss:Type="Number"><xsl:value-of select="appr_vol_sput"/></Data></Cell>
          <Cell ss:StyleID="s297a"><Data ss:Type="Number"><xsl:value-of select="appr_vol_lost"/></Data></Cell>
          <Cell ss:StyleID="s297a"><Data ss:Type="Number"><xsl:value-of select="appr_vol_heat_cw"/></Data></Cell>
          <Cell ss:StyleID="s297a"><Data ss:Type="Number"><xsl:value-of select="appr_vol_heat_ow"/></Data></Cell>
          <Cell ss:StyleID="s297a"><Data ss:Type="Number"><xsl:value-of select="appr_vol_to"/></Data></Cell>
          <Cell ss:StyleID="s297a"><Data ss:Type="Number"><xsl:value-of select="prog_floor_num"/></Data></Cell>
          <Cell ss:StyleID="s297a"><Data ss:Type="String"><xsl:value-of select="prog_ac"/></Data></Cell>
          <Cell ss:StyleID="s297a"><Data ss:Type="Number"><xsl:value-of select="prog_people_num"/></Data></Cell>
          <Cell ss:StyleID="s297a"><Data ss:Type="Number"><xsl:value-of select="prog_db_glass"/></Data></Cell>
          <Cell ss:StyleID="s297a"><Data ss:Type="Number"><xsl:value-of select="prog_plp"/></Data></Cell>
          <Cell ss:StyleID="s297a"><Data ss:Type="Number"><xsl:value-of select="prog_object_volume"/></Data></Cell>
          <Cell ss:StyleID="s297a"><Data ss:Type="Number"><xsl:value-of select="prog_total_vol"/></Data></Cell>
          <Cell ss:StyleID="s297a"><Data ss:Type="Number"><xsl:value-of select="prog_vol_heat"/></Data></Cell>
          <Cell ss:StyleID="s297a"><Data ss:Type="Number"><xsl:value-of select="prog_vol_vent"/></Data></Cell>
          <Cell ss:StyleID="s297a"><Data ss:Type="Number"><xsl:value-of select="prog_vol_sput"/></Data></Cell>
          <Cell ss:StyleID="s297a"><Data ss:Type="Number"><xsl:value-of select="prog_vol_lost"/></Data></Cell>
          <Cell ss:StyleID="s297a"><Data ss:Type="Number"><xsl:value-of select="prog_vol_heat_cw"/></Data></Cell>
          <Cell ss:StyleID="s297a"><Data ss:Type="Number"><xsl:value-of select="prog_vol_heat_ow"/></Data></Cell>
          <Cell ss:StyleID="s297a"><Data ss:Type="Number"><xsl:value-of select="prog_vol_to"/></Data></Cell>
          <Cell ss:StyleID="s297a" ss:Formula="=RC[-14]-RC[-28]"><Data ss:Type="Number"></Data></Cell>
          <Cell ss:StyleID="s297a" ss:Formula="=RC[-14]-RC[-28]"><Data ss:Type="Number"></Data></Cell>
          <Cell ss:StyleID="s297a" ss:Formula="=RC[-14]-RC[-28]"><Data ss:Type="Number"></Data></Cell>
          <Cell ss:StyleID="s297a" ss:Formula="=RC[-14]-RC[-28]"><Data ss:Type="Number"></Data></Cell>
          <Cell ss:StyleID="s297a" ss:Formula="=RC[-14]-RC[-28]"><Data ss:Type="Number"></Data></Cell>
          <Cell ss:StyleID="s297a" ss:Formula="=RC[-14]-RC[-28]"><Data ss:Type="Number"></Data></Cell>
          <Cell ss:StyleID="s297a" ss:Formula="=RC[-14]-RC[-28]"><Data ss:Type="Number"></Data></Cell>
          <Cell ss:StyleID="s297a" ss:Formula="=RC[-14]-RC[-28]"><Data ss:Type="Number"></Data></Cell>
          <Cell ss:StyleID="s297a" ss:Formula="=RC[-14]-RC[-28]"><Data ss:Type="Number"></Data></Cell>
          <Cell ss:StyleID="s297a" ss:Formula="=RC[-14]-RC[-28]"><Data ss:Type="Number"></Data></Cell>
          <Cell ss:StyleID="s297a" ss:Formula="=RC[-14]-RC[-28]"><Data ss:Type="Number"></Data></Cell>
          <Cell ss:StyleID="s297a" ss:Formula="=RC[-14]-RC[-28]"><Data ss:Type="Number"></Data></Cell>
          <Cell ss:StyleID="s297a" ss:Formula="=RC[-14]-RC[-28]"><Data ss:Type="Number"></Data></Cell>
          <Cell ss:StyleID="s297a" ss:Formula="=RC[-14]-RC[-28]"><Data ss:Type="Number"></Data></Cell>
          <Cell ss:StyleID="s297a"><Data ss:Type="String"></Data></Cell>
        </Row>
      </xsl:when>
      <xsl:when test="object_address = 'Прочие' or object_address = 'Культура' or object_address = 'Образование' or object_address = 'Прочие' or object_address = 'Муниципальный фонд' or object_address = 'Ч/сектор'">
        <Row ss:Height="12.75" ss:StyleID="s293a">
          <Cell ss:StyleID="s298a"><Data ss:Type="String"><xsl:value-of select="number"/></Data></Cell>
          <Cell ss:StyleID="s299a"><Data ss:Type="String"><xsl:value-of select="object_address"/></Data></Cell>
          <Cell ss:StyleID="s299a"><Data ss:Type="String"><xsl:value-of select="region_name"/></Data></Cell>
          <Cell ss:StyleID="s299a"><Data ss:Type="String"><xsl:value-of select="municipal_name"/></Data></Cell>
          <Cell ss:StyleID="s299a"><Data ss:Type="String"><xsl:value-of select="city_name"/></Data></Cell>
          <Cell ss:StyleID="s299a"><Data ss:Type="String"><xsl:value-of select="boiler_name"/></Data></Cell>
          <Cell ss:StyleID="s299a"><Data ss:Type="String"><xsl:value-of select="boiler_num"/></Data></Cell>
          <Cell ss:StyleID="s298a"><Data ss:Type="Number"><xsl:value-of select="input_num"/></Data></Cell>
          <Cell ss:StyleID="s298a"><Data ss:Type="String"><xsl:value-of select="input_date"/></Data></Cell>
          <Cell ss:StyleID="s298a"><Data ss:Type="String"><xsl:value-of select="ac"/></Data></Cell>
          <Cell ss:StyleID="s298a"><Data ss:Type="Number"><xsl:value-of select="heating_period"/></Data></Cell>
          <Cell ss:StyleID="s299a"><Data ss:Type="Number"><xsl:value-of select="power_total"/></Data></Cell>
          <Cell ss:StyleID="s299a"><Data ss:Type="Number"><xsl:value-of select="power_heat"/></Data></Cell>
          <Cell ss:StyleID="s299a"><Data ss:Type="Number"><xsl:value-of select="power_heat_cw"/></Data></Cell>
          <Cell ss:StyleID="s299a"><Data ss:Type="Number"><xsl:value-of select="power_heat_ow"/></Data></Cell>
          <Cell ss:StyleID="s299a"><Data ss:Type="Number"><xsl:value-of select="appr_floor_num"/></Data></Cell>
          <Cell ss:StyleID="s299a"><Data ss:Type="String"><xsl:value-of select="appr_ac"/></Data></Cell>
          <Cell ss:StyleID="s299a"><Data ss:Type="Number"><xsl:value-of select="appr_people_num"/></Data></Cell>
          <Cell ss:StyleID="s299a"><Data ss:Type="Number"><xsl:value-of select="appr_db_glass"/></Data></Cell>
          <Cell ss:StyleID="s299a"><Data ss:Type="Number"><xsl:value-of select="appr_plp"/></Data></Cell>
          <Cell ss:StyleID="s299a"><Data ss:Type="Number"><xsl:value-of select="appr_object_volume"/></Data></Cell>
          <Cell ss:StyleID="s299a"><Data ss:Type="Number"><xsl:value-of select="appr_total_vol"/></Data></Cell>
          <Cell ss:StyleID="s299a"><Data ss:Type="Number"><xsl:value-of select="appr_vol_heat"/></Data></Cell>
          <Cell ss:StyleID="s299a"><Data ss:Type="Number"><xsl:value-of select="appr_vol_vent"/></Data></Cell>
          <Cell ss:StyleID="s299a"><Data ss:Type="Number"><xsl:value-of select="appr_vol_sput"/></Data></Cell>
          <Cell ss:StyleID="s299a"><Data ss:Type="Number"><xsl:value-of select="appr_vol_lost"/></Data></Cell>
          <Cell ss:StyleID="s299a"><Data ss:Type="Number"><xsl:value-of select="appr_vol_heat_cw"/></Data></Cell>
          <Cell ss:StyleID="s299a"><Data ss:Type="Number"><xsl:value-of select="appr_vol_heat_ow"/></Data></Cell>
          <Cell ss:StyleID="s299a"><Data ss:Type="Number"><xsl:value-of select="appr_vol_to"/></Data></Cell>
          <Cell ss:StyleID="s299a"><Data ss:Type="Number"><xsl:value-of select="prog_floor_num"/></Data></Cell>
          <Cell ss:StyleID="s299a"><Data ss:Type="String"><xsl:value-of select="prog_ac"/></Data></Cell>
          <Cell ss:StyleID="s299a"><Data ss:Type="Number"><xsl:value-of select="prog_people_num"/></Data></Cell>
          <Cell ss:StyleID="s299a"><Data ss:Type="Number"><xsl:value-of select="prog_db_glass"/></Data></Cell>
          <Cell ss:StyleID="s299a"><Data ss:Type="Number"><xsl:value-of select="prog_plp"/></Data></Cell>
          <Cell ss:StyleID="s299a"><Data ss:Type="Number"><xsl:value-of select="prog_object_volume"/></Data></Cell>
          <Cell ss:StyleID="s299a"><Data ss:Type="Number"><xsl:value-of select="prog_total_vol"/></Data></Cell>
          <Cell ss:StyleID="s299a"><Data ss:Type="Number"><xsl:value-of select="prog_vol_heat"/></Data></Cell>
          <Cell ss:StyleID="s299a"><Data ss:Type="Number"><xsl:value-of select="prog_vol_vent"/></Data></Cell>
          <Cell ss:StyleID="s299a"><Data ss:Type="Number"><xsl:value-of select="prog_vol_sput"/></Data></Cell>
          <Cell ss:StyleID="s299a"><Data ss:Type="Number"><xsl:value-of select="prog_vol_lost"/></Data></Cell>
          <Cell ss:StyleID="s299a"><Data ss:Type="Number"><xsl:value-of select="prog_vol_heat_cw"/></Data></Cell>
          <Cell ss:StyleID="s299a"><Data ss:Type="Number"><xsl:value-of select="prog_vol_heat_ow"/></Data></Cell>
          <Cell ss:StyleID="s299a"><Data ss:Type="Number"><xsl:value-of select="prog_vol_to"/></Data></Cell>
          <Cell ss:StyleID="s299a" ss:Formula="=RC[-14]-RC[-28]"><Data ss:Type="Number"></Data></Cell>
          <Cell ss:StyleID="s299a" ss:Formula="=RC[-14]-RC[-28]"><Data ss:Type="Number"></Data></Cell>
          <Cell ss:StyleID="s299a" ss:Formula="=RC[-14]-RC[-28]"><Data ss:Type="Number"></Data></Cell>
          <Cell ss:StyleID="s299a" ss:Formula="=RC[-14]-RC[-28]"><Data ss:Type="Number"></Data></Cell>
          <Cell ss:StyleID="s299a" ss:Formula="=RC[-14]-RC[-28]"><Data ss:Type="Number"></Data></Cell>
          <Cell ss:StyleID="s299a" ss:Formula="=RC[-14]-RC[-28]"><Data ss:Type="Number"></Data></Cell>
          <Cell ss:StyleID="s299a" ss:Formula="=RC[-14]-RC[-28]"><Data ss:Type="Number"></Data></Cell>
          <Cell ss:StyleID="s299a" ss:Formula="=RC[-14]-RC[-28]"><Data ss:Type="Number"></Data></Cell>
          <Cell ss:StyleID="s299a" ss:Formula="=RC[-14]-RC[-28]"><Data ss:Type="Number"></Data></Cell>
          <Cell ss:StyleID="s299a" ss:Formula="=RC[-14]-RC[-28]"><Data ss:Type="Number"></Data></Cell>
          <Cell ss:StyleID="s299a" ss:Formula="=RC[-14]-RC[-28]"><Data ss:Type="Number"></Data></Cell>
          <Cell ss:StyleID="s299a" ss:Formula="=RC[-14]-RC[-28]"><Data ss:Type="Number"></Data></Cell>
          <Cell ss:StyleID="s299a" ss:Formula="=RC[-14]-RC[-28]"><Data ss:Type="Number"></Data></Cell>
          <Cell ss:StyleID="s299a" ss:Formula="=RC[-14]-RC[-28]"><Data ss:Type="Number"></Data></Cell>
          <Cell ss:StyleID="s299a"><Data ss:Type="String"></Data></Cell>
        </Row>
      </xsl:when>
      <xsl:when test="object_address != ''">
        <Row ss:Height="12.75" ss:StyleID="s300a">
          <Cell ss:StyleID="s301a"><Data ss:Type="String"><xsl:value-of select="number"/></Data></Cell>
          <Cell ss:StyleID="s302a"><Data ss:Type="String"><xsl:value-of select="object_address"/></Data></Cell>
          <Cell ss:StyleID="s302a"><Data ss:Type="String"><xsl:value-of select="region_name"/></Data></Cell>
          <Cell ss:StyleID="s302a"><Data ss:Type="String"><xsl:value-of select="municipal_name"/></Data></Cell>
          <Cell ss:StyleID="s302a"><Data ss:Type="String"><xsl:value-of select="city_name"/></Data></Cell>
          <Cell ss:StyleID="s302a"><Data ss:Type="String"><xsl:value-of select="boiler_name"/></Data></Cell>
          <Cell ss:StyleID="s302a"><Data ss:Type="String"><xsl:value-of select="boiler_num"/></Data></Cell>
          <Cell ss:StyleID="s301a"><Data ss:Type="Number"><xsl:value-of select="input_num"/></Data></Cell>
          <Cell ss:StyleID="s301a"><Data ss:Type="String"><xsl:value-of select="input_date"/></Data></Cell>
          <Cell ss:StyleID="s301a"><Data ss:Type="String"><xsl:value-of select="ac"/></Data></Cell>
          <Cell ss:StyleID="s301a"><Data ss:Type="Number"><xsl:value-of select="heating_period"/></Data></Cell>
          <Cell ss:StyleID="s302a"><Data ss:Type="Number"><xsl:value-of select="power_total"/></Data></Cell>
          <Cell ss:StyleID="s302a"><Data ss:Type="Number"><xsl:value-of select="power_heat"/></Data></Cell>
          <Cell ss:StyleID="s302a"><Data ss:Type="Number"><xsl:value-of select="power_heat_cw"/></Data></Cell>
          <Cell ss:StyleID="s302a"><Data ss:Type="Number"><xsl:value-of select="power_heat_ow"/></Data></Cell>
          <Cell ss:StyleID="s302a"><Data ss:Type="Number"><xsl:value-of select="appr_floor_num"/></Data></Cell>
          <Cell ss:StyleID="s302a"><Data ss:Type="String"><xsl:value-of select="appr_ac"/></Data></Cell>
          <Cell ss:StyleID="s302a"><Data ss:Type="Number"><xsl:value-of select="appr_people_num"/></Data></Cell>
          <Cell ss:StyleID="s302a"><Data ss:Type="Number"><xsl:value-of select="appr_db_glass"/></Data></Cell>
          <Cell ss:StyleID="s302a"><Data ss:Type="Number"><xsl:value-of select="appr_plp"/></Data></Cell>
          <Cell ss:StyleID="s302a"><Data ss:Type="Number"><xsl:value-of select="appr_object_volume"/></Data></Cell>
          <Cell ss:StyleID="s302a"><Data ss:Type="Number"><xsl:value-of select="appr_total_vol"/></Data></Cell>
          <Cell ss:StyleID="s302a"><Data ss:Type="Number"><xsl:value-of select="appr_vol_heat"/></Data></Cell>
          <Cell ss:StyleID="s302a"><Data ss:Type="Number"><xsl:value-of select="appr_vol_vent"/></Data></Cell>
          <Cell ss:StyleID="s302a"><Data ss:Type="Number"><xsl:value-of select="appr_vol_sput"/></Data></Cell>
          <Cell ss:StyleID="s302a"><Data ss:Type="Number"><xsl:value-of select="appr_vol_lost"/></Data></Cell>
          <Cell ss:StyleID="s302a"><Data ss:Type="Number"><xsl:value-of select="appr_vol_heat_cw"/></Data></Cell>
          <Cell ss:StyleID="s302a"><Data ss:Type="Number"><xsl:value-of select="appr_vol_heat_ow"/></Data></Cell>
          <Cell ss:StyleID="s302a"><Data ss:Type="Number"><xsl:value-of select="appr_vol_to"/></Data></Cell>
          <Cell ss:StyleID="s302a"><Data ss:Type="Number"><xsl:value-of select="prog_floor_num"/></Data></Cell>
          <Cell ss:StyleID="s302a"><Data ss:Type="String"><xsl:value-of select="prog_ac"/></Data></Cell>
          <Cell ss:StyleID="s302a"><Data ss:Type="Number"><xsl:value-of select="prog_people_num"/></Data></Cell>
          <Cell ss:StyleID="s302a"><Data ss:Type="Number"><xsl:value-of select="prog_db_glass"/></Data></Cell>
          <Cell ss:StyleID="s302a"><Data ss:Type="Number"><xsl:value-of select="prog_plp"/></Data></Cell>
          <Cell ss:StyleID="s302a"><Data ss:Type="Number"><xsl:value-of select="prog_object_volume"/></Data></Cell>
          <Cell ss:StyleID="s302a"><Data ss:Type="Number"><xsl:value-of select="prog_total_vol"/></Data></Cell>
          <Cell ss:StyleID="s302a"><Data ss:Type="Number"><xsl:value-of select="prog_vol_heat"/></Data></Cell>
          <Cell ss:StyleID="s302a"><Data ss:Type="Number"><xsl:value-of select="prog_vol_vent"/></Data></Cell>
          <Cell ss:StyleID="s302a"><Data ss:Type="Number"><xsl:value-of select="prog_vol_sput"/></Data></Cell>
          <Cell ss:StyleID="s302a"><Data ss:Type="Number"><xsl:value-of select="prog_vol_lost"/></Data></Cell>
          <Cell ss:StyleID="s302a"><Data ss:Type="Number"><xsl:value-of select="prog_vol_heat_cw"/></Data></Cell>
          <Cell ss:StyleID="s302a"><Data ss:Type="Number"><xsl:value-of select="prog_vol_heat_ow"/></Data></Cell>
          <Cell ss:StyleID="s302a"><Data ss:Type="Number"><xsl:value-of select="prog_vol_to"/></Data></Cell>
          <Cell ss:StyleID="s302a" ss:Formula="=RC[-14]-RC[-28]"><Data ss:Type="Number"></Data></Cell>
          <Cell ss:StyleID="s302a" ss:Formula="=RC[-14]-RC[-28]"><Data ss:Type="Number"></Data></Cell>
          <Cell ss:StyleID="s302a" ss:Formula="=RC[-14]-RC[-28]"><Data ss:Type="Number"></Data></Cell>
          <Cell ss:StyleID="s302a" ss:Formula="=RC[-14]-RC[-28]"><Data ss:Type="Number"></Data></Cell>
          <Cell ss:StyleID="s302a" ss:Formula="=RC[-14]-RC[-28]"><Data ss:Type="Number"></Data></Cell>
          <Cell ss:StyleID="s302a" ss:Formula="=RC[-14]-RC[-28]"><Data ss:Type="Number"></Data></Cell>
          <Cell ss:StyleID="s302a" ss:Formula="=RC[-14]-RC[-28]"><Data ss:Type="Number"></Data></Cell>
          <Cell ss:StyleID="s302a" ss:Formula="=RC[-14]-RC[-28]"><Data ss:Type="Number"></Data></Cell>
          <Cell ss:StyleID="s302a" ss:Formula="=RC[-14]-RC[-28]"><Data ss:Type="Number"></Data></Cell>
          <Cell ss:StyleID="s302a" ss:Formula="=RC[-14]-RC[-28]"><Data ss:Type="Number"></Data></Cell>
          <Cell ss:StyleID="s302a" ss:Formula="=RC[-14]-RC[-28]"><Data ss:Type="Number"></Data></Cell>
          <Cell ss:StyleID="s302a" ss:Formula="=RC[-14]-RC[-28]"><Data ss:Type="Number"></Data></Cell>
          <Cell ss:StyleID="s302a" ss:Formula="=RC[-14]-RC[-28]"><Data ss:Type="Number"></Data></Cell>
          <Cell ss:StyleID="s302a" ss:Formula="=RC[-14]-RC[-28]"><Data ss:Type="Number"></Data></Cell>
          <Cell ss:StyleID="s302a"><Data ss:Type="String"></Data></Cell>
        </Row>
      </xsl:when>
    </xsl:choose>
  </xsl:template>
  <!-- <<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<< Шаблон данных листа ТХ2 >>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>> -->
  <xsl:template match="TS2">
    <Row>
      <Cell ss:Index="2" ss:StyleID="s91"><Data ss:Type="String"><xsl:value-of select="reg" /></Data></Cell>
      <Cell ss:StyleID="s91"><Data ss:Type="String"><xsl:value-of select="city" /></Data></Cell>
      <Cell ss:StyleID="s91"><Data ss:Type="String"><xsl:value-of select="Boiler" /></Data></Cell>
      <Cell ss:StyleID="s91"><Data ss:Type="String"><xsl:value-of select="typ" /></Data></Cell>
      <Cell ss:StyleID="s91"><Data ss:Type="String"><xsl:value-of select="name" /></Data></Cell>
      <Cell ss:StyleID="s91"><Data ss:Type="String"><xsl:value-of select="strt" /></Data></Cell>
      <Cell ss:StyleID="s91"><Data ss:Type="Number"><xsl:value-of select="et" /></Data></Cell>
      <Cell ss:StyleID="s91"><Data ss:Type="Number"><xsl:value-of select="sqr" /></Data></Cell>
      <Cell ss:StyleID="s91"><Data ss:Type="Number"><xsl:value-of select="V" /></Data></Cell>
      <Cell ss:StyleID="s91"><Data ss:Type="Number"><xsl:value-of select="tempV" /></Data></Cell>
      <Cell ss:StyleID="s91"><Data ss:Type="Number"><xsl:value-of select="man" /></Data></Cell>
      <Cell ss:StyleID="s91"><Data ss:Type="Number"><xsl:value-of select="norm_water" /></Data></Cell>
      <Cell ss:StyleID="s91"><Data ss:Type="Number"><xsl:value-of select="vol" /></Data></Cell>
      <Cell ss:StyleID="s91"><Data ss:Type="Number"><xsl:value-of select="sumq" /></Data></Cell>
      <Cell ss:StyleID="s91"><Data ss:Type="Number"><xsl:value-of select="Vq" /></Data></Cell>
      <Cell ss:StyleID="s91"><Data ss:Type="Number"><xsl:value-of select="Vchvs" /></Data></Cell>
      <Cell ss:StyleID="s91"><Data ss:Type="Number"><xsl:value-of select="Vshvs" /></Data></Cell>
      <Cell ss:StyleID="s91"><Data ss:Type="Number"><xsl:value-of select="sumQ" /></Data></Cell>
      <Cell ss:StyleID="s91"><Data ss:Type="Number"><xsl:value-of select="sQ" /></Data></Cell>
      <Cell ss:StyleID="s91"><Data ss:Type="Number"><xsl:value-of select="sshvs" /></Data></Cell>
      <Cell ss:StyleID="s91"><Data ss:Type="Number"><xsl:value-of select="schvs" /></Data></Cell>
      <Cell ss:StyleID="s91"><Data ss:Type="Number"><xsl:value-of select="sQs" /></Data></Cell>
      <Cell ss:StyleID="s91"><Data ss:Type="Number"><xsl:value-of select="sQt" /></Data></Cell>
      <Cell ss:StyleID="s91"><Data ss:Type="Number"><xsl:value-of select="cdays" /></Data></Cell>
      <Cell ss:StyleID="s91"><Data ss:Type="String">-</Data></Cell>
    </Row>
  </xsl:template>
  <!-- <<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<< Шаблон данных листа ТХ3 >>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>> -->
  <xsl:template match="TS3">
    <xsl:choose>
      <xsl:when test="object_address = 'ВСЕГО:'">
        <Row ss:Height="12.75" ss:StyleID="s293a">
          <Cell ss:StyleID="s294a"><Data ss:Type="String"><xsl:value-of select="number"/></Data></Cell>
          <Cell ss:StyleID="s295a"><Data ss:Type="String"><xsl:value-of select="object_address"/></Data></Cell>
          <Cell ss:StyleID="s295a"><Data ss:Type="String"><xsl:value-of select="region_name"/></Data></Cell>
          <Cell ss:StyleID="s295a"><Data ss:Type="String"><xsl:value-of select="municipal_name"/></Data></Cell>
          <Cell ss:StyleID="s295a"><Data ss:Type="String"><xsl:value-of select="city_name"/></Data></Cell>
          <Cell ss:StyleID="s295a"><Data ss:Type="String"><xsl:value-of select="boiler_name"/></Data></Cell>
          <Cell ss:StyleID="s295a"><Data ss:Type="String"><xsl:value-of select="boiler_num"/></Data></Cell>
          <Cell ss:StyleID="s294a"><Data ss:Type="Number"><xsl:value-of select="input_num"/></Data></Cell>
          <Cell ss:StyleID="s294a"><Data ss:Type="String"><xsl:value-of select="input_date"/></Data></Cell>
          <Cell ss:StyleID="s294a"><Data ss:Type="String"><xsl:value-of select="ac"/></Data></Cell>
          <Cell ss:StyleID="s294a"><Data ss:Type="Number"><xsl:value-of select="heating_period"/></Data></Cell>
          <Cell ss:StyleID="s295a"><Data ss:Type="Number"><xsl:value-of select="power_total"/></Data></Cell>
          <Cell ss:StyleID="s295a"><Data ss:Type="Number"><xsl:value-of select="power_heat"/></Data></Cell>
          <Cell ss:StyleID="s295a"><Data ss:Type="Number"><xsl:value-of select="power_heat_cw"/></Data></Cell>
          <Cell ss:StyleID="s295a"><Data ss:Type="Number"><xsl:value-of select="power_heat_ow"/></Data></Cell>
          <Cell ss:StyleID="s295a"><Data ss:Type="Number"><xsl:value-of select="appr_floor_num"/></Data></Cell>
          <Cell ss:StyleID="s295a"><Data ss:Type="String"><xsl:value-of select="appr_ac"/></Data></Cell>
          <Cell ss:StyleID="s295a"><Data ss:Type="Number"><xsl:value-of select="appr_people_num"/></Data></Cell>
          <Cell ss:StyleID="s295a"><Data ss:Type="Number"><xsl:value-of select="appr_db_glass"/></Data></Cell>
          <Cell ss:StyleID="s295a"><Data ss:Type="Number"><xsl:value-of select="appr_plp"/></Data></Cell>
          <Cell ss:StyleID="s295a"><Data ss:Type="Number"><xsl:value-of select="appr_object_volume"/></Data></Cell>
          <Cell ss:StyleID="s295a"><Data ss:Type="Number"><xsl:value-of select="appr_total_vol"/></Data></Cell>
          <Cell ss:StyleID="s295a"><Data ss:Type="Number"><xsl:value-of select="appr_vol_heat"/></Data></Cell>
          <Cell ss:StyleID="s295a"><Data ss:Type="Number"><xsl:value-of select="appr_vol_vent"/></Data></Cell>
          <Cell ss:StyleID="s295a"><Data ss:Type="Number"><xsl:value-of select="appr_vol_sput"/></Data></Cell>
          <Cell ss:StyleID="s295a"><Data ss:Type="Number"><xsl:value-of select="appr_vol_lost"/></Data></Cell>
          <Cell ss:StyleID="s295a"><Data ss:Type="Number"><xsl:value-of select="appr_vol_heat_cw"/></Data></Cell>
          <Cell ss:StyleID="s295a"><Data ss:Type="Number"><xsl:value-of select="appr_vol_heat_ow"/></Data></Cell>
          <Cell ss:StyleID="s295a"><Data ss:Type="Number"><xsl:value-of select="appr_vol_to"/></Data></Cell>
          <Cell ss:StyleID="s295a"><Data ss:Type="Number"><xsl:value-of select="prog_floor_num"/></Data></Cell>
          <Cell ss:StyleID="s295a"><Data ss:Type="String"><xsl:value-of select="prog_ac"/></Data></Cell>
          <Cell ss:StyleID="s295a"><Data ss:Type="Number"><xsl:value-of select="prog_people_num"/></Data></Cell>
          <Cell ss:StyleID="s295a"><Data ss:Type="Number"><xsl:value-of select="prog_db_glass"/></Data></Cell>
          <Cell ss:StyleID="s295a"><Data ss:Type="Number"><xsl:value-of select="prog_plp"/></Data></Cell>
          <Cell ss:StyleID="s295a"><Data ss:Type="Number"><xsl:value-of select="prog_object_volume"/></Data></Cell>
          <Cell ss:StyleID="s295a"><Data ss:Type="Number"><xsl:value-of select="prog_total_vol"/></Data></Cell>
          <Cell ss:StyleID="s295a"><Data ss:Type="Number"><xsl:value-of select="prog_vol_heat"/></Data></Cell>
          <Cell ss:StyleID="s295a"><Data ss:Type="Number"><xsl:value-of select="prog_vol_vent"/></Data></Cell>
          <Cell ss:StyleID="s295a"><Data ss:Type="Number"><xsl:value-of select="prog_vol_sput"/></Data></Cell>
          <Cell ss:StyleID="s295a"><Data ss:Type="Number"><xsl:value-of select="prog_vol_lost"/></Data></Cell>
          <Cell ss:StyleID="s295a"><Data ss:Type="Number"><xsl:value-of select="prog_vol_heat_cw"/></Data></Cell>
          <Cell ss:StyleID="s295a"><Data ss:Type="Number"><xsl:value-of select="prog_vol_heat_ow"/></Data></Cell>
          <Cell ss:StyleID="s295a"><Data ss:Type="Number"><xsl:value-of select="prog_vol_to"/></Data></Cell>
          <Cell ss:StyleID="s295a" ss:Formula="=RC[-14]-RC[-28]"><Data ss:Type="Number"></Data></Cell>
          <Cell ss:StyleID="s295a" ss:Formula="=RC[-14]-RC[-28]"><Data ss:Type="Number"></Data></Cell>
          <Cell ss:StyleID="s295a" ss:Formula="=RC[-14]-RC[-28]"><Data ss:Type="Number"></Data></Cell>
          <Cell ss:StyleID="s295a" ss:Formula="=RC[-14]-RC[-28]"><Data ss:Type="Number"></Data></Cell>
          <Cell ss:StyleID="s295a" ss:Formula="=RC[-14]-RC[-28]"><Data ss:Type="Number"></Data></Cell>
          <Cell ss:StyleID="s295a" ss:Formula="=RC[-14]-RC[-28]"><Data ss:Type="Number"></Data></Cell>
          <Cell ss:StyleID="s295a" ss:Formula="=RC[-14]-RC[-28]"><Data ss:Type="Number"></Data></Cell>
          <Cell ss:StyleID="s295a" ss:Formula="=RC[-14]-RC[-28]"><Data ss:Type="Number"></Data></Cell>
          <Cell ss:StyleID="s295a" ss:Formula="=RC[-14]-RC[-28]"><Data ss:Type="Number"></Data></Cell>
          <Cell ss:StyleID="s295a" ss:Formula="=RC[-14]-RC[-28]"><Data ss:Type="Number"></Data></Cell>
          <Cell ss:StyleID="s295a" ss:Formula="=RC[-14]-RC[-28]"><Data ss:Type="Number"></Data></Cell>
          <Cell ss:StyleID="s295a" ss:Formula="=RC[-14]-RC[-28]"><Data ss:Type="Number"></Data></Cell>
          <Cell ss:StyleID="s295a" ss:Formula="=RC[-14]-RC[-28]"><Data ss:Type="Number"></Data></Cell>
          <Cell ss:StyleID="s295a" ss:Formula="=RC[-14]-RC[-28]"><Data ss:Type="Number"></Data></Cell>
          <Cell ss:StyleID="s295a"><Data ss:Type="String"></Data></Cell>
        </Row>
      </xsl:when>
      <xsl:when test="number = 1 or number = 2 or number = 3 or number = 4 or number = 5 or number = 6">
        <Row ss:Height="12.75" ss:StyleID="s293a">
          <Cell ss:StyleID="s296a"><Data ss:Type="String"><xsl:value-of select="number"/></Data></Cell>
          <Cell ss:StyleID="s297a"><Data ss:Type="String"><xsl:value-of select="object_address"/></Data></Cell>
          <Cell ss:StyleID="s297a"><Data ss:Type="String"><xsl:value-of select="region_name"/></Data></Cell>
          <Cell ss:StyleID="s297a"><Data ss:Type="String"><xsl:value-of select="municipal_name"/></Data></Cell>
          <Cell ss:StyleID="s297a"><Data ss:Type="String"><xsl:value-of select="city_name"/></Data></Cell>
          <Cell ss:StyleID="s297a"><Data ss:Type="String"><xsl:value-of select="boiler_name"/></Data></Cell>
          <Cell ss:StyleID="s297a"><Data ss:Type="String"><xsl:value-of select="boiler_num"/></Data></Cell>
          <Cell ss:StyleID="s296a"><Data ss:Type="Number"><xsl:value-of select="input_num"/></Data></Cell>
          <Cell ss:StyleID="s296a"><Data ss:Type="String"><xsl:value-of select="input_date"/></Data></Cell>
          <Cell ss:StyleID="s296a"><Data ss:Type="String"><xsl:value-of select="ac"/></Data></Cell>
          <Cell ss:StyleID="s296a"><Data ss:Type="Number"><xsl:value-of select="heating_period"/></Data></Cell>
          <Cell ss:StyleID="s297a"><Data ss:Type="Number"><xsl:value-of select="power_total"/></Data></Cell>
          <Cell ss:StyleID="s297a"><Data ss:Type="Number"><xsl:value-of select="power_heat"/></Data></Cell>
          <Cell ss:StyleID="s297a"><Data ss:Type="Number"><xsl:value-of select="power_heat_cw"/></Data></Cell>
          <Cell ss:StyleID="s297a"><Data ss:Type="Number"><xsl:value-of select="power_heat_ow"/></Data></Cell>
          <Cell ss:StyleID="s297a"><Data ss:Type="Number"><xsl:value-of select="appr_floor_num"/></Data></Cell>
          <Cell ss:StyleID="s297a"><Data ss:Type="String"><xsl:value-of select="appr_ac"/></Data></Cell>
          <Cell ss:StyleID="s297a"><Data ss:Type="Number"><xsl:value-of select="appr_people_num"/></Data></Cell>
          <Cell ss:StyleID="s297a"><Data ss:Type="Number"><xsl:value-of select="appr_db_glass"/></Data></Cell>
          <Cell ss:StyleID="s297a"><Data ss:Type="Number"><xsl:value-of select="appr_plp"/></Data></Cell>
          <Cell ss:StyleID="s297a"><Data ss:Type="Number"><xsl:value-of select="appr_object_volume"/></Data></Cell>
          <Cell ss:StyleID="s297a"><Data ss:Type="Number"><xsl:value-of select="appr_total_vol"/></Data></Cell>
          <Cell ss:StyleID="s297a"><Data ss:Type="Number"><xsl:value-of select="appr_vol_heat"/></Data></Cell>
          <Cell ss:StyleID="s297a"><Data ss:Type="Number"><xsl:value-of select="appr_vol_vent"/></Data></Cell>
          <Cell ss:StyleID="s297a"><Data ss:Type="Number"><xsl:value-of select="appr_vol_sput"/></Data></Cell>
          <Cell ss:StyleID="s297a"><Data ss:Type="Number"><xsl:value-of select="appr_vol_lost"/></Data></Cell>
          <Cell ss:StyleID="s297a"><Data ss:Type="Number"><xsl:value-of select="appr_vol_heat_cw"/></Data></Cell>
          <Cell ss:StyleID="s297a"><Data ss:Type="Number"><xsl:value-of select="appr_vol_heat_ow"/></Data></Cell>
          <Cell ss:StyleID="s297a"><Data ss:Type="Number"><xsl:value-of select="appr_vol_to"/></Data></Cell>
          <Cell ss:StyleID="s297a"><Data ss:Type="Number"><xsl:value-of select="prog_floor_num"/></Data></Cell>
          <Cell ss:StyleID="s297a"><Data ss:Type="String"><xsl:value-of select="prog_ac"/></Data></Cell>
          <Cell ss:StyleID="s297a"><Data ss:Type="Number"><xsl:value-of select="prog_people_num"/></Data></Cell>
          <Cell ss:StyleID="s297a"><Data ss:Type="Number"><xsl:value-of select="prog_db_glass"/></Data></Cell>
          <Cell ss:StyleID="s297a"><Data ss:Type="Number"><xsl:value-of select="prog_plp"/></Data></Cell>
          <Cell ss:StyleID="s297a"><Data ss:Type="Number"><xsl:value-of select="prog_object_volume"/></Data></Cell>
          <Cell ss:StyleID="s297a"><Data ss:Type="Number"><xsl:value-of select="prog_total_vol"/></Data></Cell>
          <Cell ss:StyleID="s297a"><Data ss:Type="Number"><xsl:value-of select="prog_vol_heat"/></Data></Cell>
          <Cell ss:StyleID="s297a"><Data ss:Type="Number"><xsl:value-of select="prog_vol_vent"/></Data></Cell>
          <Cell ss:StyleID="s297a"><Data ss:Type="Number"><xsl:value-of select="prog_vol_sput"/></Data></Cell>
          <Cell ss:StyleID="s297a"><Data ss:Type="Number"><xsl:value-of select="prog_vol_lost"/></Data></Cell>
          <Cell ss:StyleID="s297a"><Data ss:Type="Number"><xsl:value-of select="prog_vol_heat_cw"/></Data></Cell>
          <Cell ss:StyleID="s297a"><Data ss:Type="Number"><xsl:value-of select="prog_vol_heat_ow"/></Data></Cell>
          <Cell ss:StyleID="s297a"><Data ss:Type="Number"><xsl:value-of select="prog_vol_to"/></Data></Cell>
          <Cell ss:StyleID="s297a" ss:Formula="=RC[-14]-RC[-28]"><Data ss:Type="Number"></Data></Cell>
          <Cell ss:StyleID="s297a" ss:Formula="=RC[-14]-RC[-28]"><Data ss:Type="Number"></Data></Cell>
          <Cell ss:StyleID="s297a" ss:Formula="=RC[-14]-RC[-28]"><Data ss:Type="Number"></Data></Cell>
          <Cell ss:StyleID="s297a" ss:Formula="=RC[-14]-RC[-28]"><Data ss:Type="Number"></Data></Cell>
          <Cell ss:StyleID="s297a" ss:Formula="=RC[-14]-RC[-28]"><Data ss:Type="Number"></Data></Cell>
          <Cell ss:StyleID="s297a" ss:Formula="=RC[-14]-RC[-28]"><Data ss:Type="Number"></Data></Cell>
          <Cell ss:StyleID="s297a" ss:Formula="=RC[-14]-RC[-28]"><Data ss:Type="Number"></Data></Cell>
          <Cell ss:StyleID="s297a" ss:Formula="=RC[-14]-RC[-28]"><Data ss:Type="Number"></Data></Cell>
          <Cell ss:StyleID="s297a" ss:Formula="=RC[-14]-RC[-28]"><Data ss:Type="Number"></Data></Cell>
          <Cell ss:StyleID="s297a" ss:Formula="=RC[-14]-RC[-28]"><Data ss:Type="Number"></Data></Cell>
          <Cell ss:StyleID="s297a" ss:Formula="=RC[-14]-RC[-28]"><Data ss:Type="Number"></Data></Cell>
          <Cell ss:StyleID="s297a" ss:Formula="=RC[-14]-RC[-28]"><Data ss:Type="Number"></Data></Cell>
          <Cell ss:StyleID="s297a" ss:Formula="=RC[-14]-RC[-28]"><Data ss:Type="Number"></Data></Cell>
          <Cell ss:StyleID="s297a" ss:Formula="=RC[-14]-RC[-28]"><Data ss:Type="Number"></Data></Cell>
          <Cell ss:StyleID="s297a"><Data ss:Type="String"></Data></Cell>
        </Row>
      </xsl:when>
      <xsl:when test="object_address = 'Прочие' or object_address = 'Культура' or object_address = 'Образование' or object_address = 'Прочие' or object_address = 'Муниципальный фонд' or object_address = 'Ч/сектор'">
        <Row ss:Height="12.75" ss:StyleID="s293a">
          <Cell ss:StyleID="s298a"><Data ss:Type="String"><xsl:value-of select="number"/></Data></Cell>
          <Cell ss:StyleID="s299a"><Data ss:Type="String"><xsl:value-of select="object_address"/></Data></Cell>
          <Cell ss:StyleID="s299a"><Data ss:Type="String"><xsl:value-of select="region_name"/></Data></Cell>
          <Cell ss:StyleID="s299a"><Data ss:Type="String"><xsl:value-of select="municipal_name"/></Data></Cell>
          <Cell ss:StyleID="s299a"><Data ss:Type="String"><xsl:value-of select="city_name"/></Data></Cell>
          <Cell ss:StyleID="s299a"><Data ss:Type="String"><xsl:value-of select="boiler_name"/></Data></Cell>
          <Cell ss:StyleID="s299a"><Data ss:Type="String"><xsl:value-of select="boiler_num"/></Data></Cell>
          <Cell ss:StyleID="s298a"><Data ss:Type="Number"><xsl:value-of select="input_num"/></Data></Cell>
          <Cell ss:StyleID="s298a"><Data ss:Type="String"><xsl:value-of select="input_date"/></Data></Cell>
          <Cell ss:StyleID="s298a"><Data ss:Type="String"><xsl:value-of select="ac"/></Data></Cell>
          <Cell ss:StyleID="s298a"><Data ss:Type="Number"><xsl:value-of select="heating_period"/></Data></Cell>
          <Cell ss:StyleID="s299a"><Data ss:Type="Number"><xsl:value-of select="power_total"/></Data></Cell>
          <Cell ss:StyleID="s299a"><Data ss:Type="Number"><xsl:value-of select="power_heat"/></Data></Cell>
          <Cell ss:StyleID="s299a"><Data ss:Type="Number"><xsl:value-of select="power_heat_cw"/></Data></Cell>
          <Cell ss:StyleID="s299a"><Data ss:Type="Number"><xsl:value-of select="power_heat_ow"/></Data></Cell>
          <Cell ss:StyleID="s299a"><Data ss:Type="Number"><xsl:value-of select="appr_floor_num"/></Data></Cell>
          <Cell ss:StyleID="s299a"><Data ss:Type="String"><xsl:value-of select="appr_ac"/></Data></Cell>
          <Cell ss:StyleID="s299a"><Data ss:Type="Number"><xsl:value-of select="appr_people_num"/></Data></Cell>
          <Cell ss:StyleID="s299a"><Data ss:Type="Number"><xsl:value-of select="appr_db_glass"/></Data></Cell>
          <Cell ss:StyleID="s299a"><Data ss:Type="Number"><xsl:value-of select="appr_plp"/></Data></Cell>
          <Cell ss:StyleID="s299a"><Data ss:Type="Number"><xsl:value-of select="appr_object_volume"/></Data></Cell>
          <Cell ss:StyleID="s299a"><Data ss:Type="Number"><xsl:value-of select="appr_total_vol"/></Data></Cell>
          <Cell ss:StyleID="s299a"><Data ss:Type="Number"><xsl:value-of select="appr_vol_heat"/></Data></Cell>
          <Cell ss:StyleID="s299a"><Data ss:Type="Number"><xsl:value-of select="appr_vol_vent"/></Data></Cell>
          <Cell ss:StyleID="s299a"><Data ss:Type="Number"><xsl:value-of select="appr_vol_sput"/></Data></Cell>
          <Cell ss:StyleID="s299a"><Data ss:Type="Number"><xsl:value-of select="appr_vol_lost"/></Data></Cell>
          <Cell ss:StyleID="s299a"><Data ss:Type="Number"><xsl:value-of select="appr_vol_heat_cw"/></Data></Cell>
          <Cell ss:StyleID="s299a"><Data ss:Type="Number"><xsl:value-of select="appr_vol_heat_ow"/></Data></Cell>
          <Cell ss:StyleID="s299a"><Data ss:Type="Number"><xsl:value-of select="appr_vol_to"/></Data></Cell>
          <Cell ss:StyleID="s299a"><Data ss:Type="Number"><xsl:value-of select="prog_floor_num"/></Data></Cell>
          <Cell ss:StyleID="s299a"><Data ss:Type="String"><xsl:value-of select="prog_ac"/></Data></Cell>
          <Cell ss:StyleID="s299a"><Data ss:Type="Number"><xsl:value-of select="prog_people_num"/></Data></Cell>
          <Cell ss:StyleID="s299a"><Data ss:Type="Number"><xsl:value-of select="prog_db_glass"/></Data></Cell>
          <Cell ss:StyleID="s299a"><Data ss:Type="Number"><xsl:value-of select="prog_plp"/></Data></Cell>
          <Cell ss:StyleID="s299a"><Data ss:Type="Number"><xsl:value-of select="prog_object_volume"/></Data></Cell>
          <Cell ss:StyleID="s299a"><Data ss:Type="Number"><xsl:value-of select="prog_total_vol"/></Data></Cell>
          <Cell ss:StyleID="s299a"><Data ss:Type="Number"><xsl:value-of select="prog_vol_heat"/></Data></Cell>
          <Cell ss:StyleID="s299a"><Data ss:Type="Number"><xsl:value-of select="prog_vol_vent"/></Data></Cell>
          <Cell ss:StyleID="s299a"><Data ss:Type="Number"><xsl:value-of select="prog_vol_sput"/></Data></Cell>
          <Cell ss:StyleID="s299a"><Data ss:Type="Number"><xsl:value-of select="prog_vol_lost"/></Data></Cell>
          <Cell ss:StyleID="s299a"><Data ss:Type="Number"><xsl:value-of select="prog_vol_heat_cw"/></Data></Cell>
          <Cell ss:StyleID="s299a"><Data ss:Type="Number"><xsl:value-of select="prog_vol_heat_ow"/></Data></Cell>
          <Cell ss:StyleID="s299a"><Data ss:Type="Number"><xsl:value-of select="prog_vol_to"/></Data></Cell>
          <Cell ss:StyleID="s299a" ss:Formula="=RC[-14]-RC[-28]"><Data ss:Type="Number"></Data></Cell>
          <Cell ss:StyleID="s299a" ss:Formula="=RC[-14]-RC[-28]"><Data ss:Type="Number"></Data></Cell>
          <Cell ss:StyleID="s299a" ss:Formula="=RC[-14]-RC[-28]"><Data ss:Type="Number"></Data></Cell>
          <Cell ss:StyleID="s299a" ss:Formula="=RC[-14]-RC[-28]"><Data ss:Type="Number"></Data></Cell>
          <Cell ss:StyleID="s299a" ss:Formula="=RC[-14]-RC[-28]"><Data ss:Type="Number"></Data></Cell>
          <Cell ss:StyleID="s299a" ss:Formula="=RC[-14]-RC[-28]"><Data ss:Type="Number"></Data></Cell>
          <Cell ss:StyleID="s299a" ss:Formula="=RC[-14]-RC[-28]"><Data ss:Type="Number"></Data></Cell>
          <Cell ss:StyleID="s299a" ss:Formula="=RC[-14]-RC[-28]"><Data ss:Type="Number"></Data></Cell>
          <Cell ss:StyleID="s299a" ss:Formula="=RC[-14]-RC[-28]"><Data ss:Type="Number"></Data></Cell>
          <Cell ss:StyleID="s299a" ss:Formula="=RC[-14]-RC[-28]"><Data ss:Type="Number"></Data></Cell>
          <Cell ss:StyleID="s299a" ss:Formula="=RC[-14]-RC[-28]"><Data ss:Type="Number"></Data></Cell>
          <Cell ss:StyleID="s299a" ss:Formula="=RC[-14]-RC[-28]"><Data ss:Type="Number"></Data></Cell>
          <Cell ss:StyleID="s299a" ss:Formula="=RC[-14]-RC[-28]"><Data ss:Type="Number"></Data></Cell>
          <Cell ss:StyleID="s299a" ss:Formula="=RC[-14]-RC[-28]"><Data ss:Type="Number"></Data></Cell>
          <Cell ss:StyleID="s299a"><Data ss:Type="String"></Data></Cell>
        </Row>
      </xsl:when>
      <xsl:when test="object_address != ''">
        <Row ss:Height="12.75" ss:StyleID="s300a">
          <Cell ss:StyleID="s301a"><Data ss:Type="String"><xsl:value-of select="number"/></Data></Cell>
          <Cell ss:StyleID="s302a"><Data ss:Type="String"><xsl:value-of select="object_address"/></Data></Cell>
          <Cell ss:StyleID="s302a"><Data ss:Type="String"><xsl:value-of select="region_name"/></Data></Cell>
          <Cell ss:StyleID="s302a"><Data ss:Type="String"><xsl:value-of select="municipal_name"/></Data></Cell>
          <Cell ss:StyleID="s302a"><Data ss:Type="String"><xsl:value-of select="city_name"/></Data></Cell>
          <Cell ss:StyleID="s302a"><Data ss:Type="String"><xsl:value-of select="boiler_name"/></Data></Cell>
          <Cell ss:StyleID="s302a"><Data ss:Type="String"><xsl:value-of select="boiler_num"/></Data></Cell>
          <Cell ss:StyleID="s301a"><Data ss:Type="Number"><xsl:value-of select="input_num"/></Data></Cell>
          <Cell ss:StyleID="s301a"><Data ss:Type="String"><xsl:value-of select="input_date"/></Data></Cell>
          <Cell ss:StyleID="s301a"><Data ss:Type="String"><xsl:value-of select="ac"/></Data></Cell>
          <Cell ss:StyleID="s301a"><Data ss:Type="Number"><xsl:value-of select="heating_period"/></Data></Cell>
          <Cell ss:StyleID="s302a"><Data ss:Type="Number"><xsl:value-of select="power_total"/></Data></Cell>
          <Cell ss:StyleID="s302a"><Data ss:Type="Number"><xsl:value-of select="power_heat"/></Data></Cell>
          <Cell ss:StyleID="s302a"><Data ss:Type="Number"><xsl:value-of select="power_heat_cw"/></Data></Cell>
          <Cell ss:StyleID="s302a"><Data ss:Type="Number"><xsl:value-of select="power_heat_ow"/></Data></Cell>
          <Cell ss:StyleID="s302a"><Data ss:Type="Number"><xsl:value-of select="appr_floor_num"/></Data></Cell>
          <Cell ss:StyleID="s302a"><Data ss:Type="String"><xsl:value-of select="appr_ac"/></Data></Cell>
          <Cell ss:StyleID="s302a"><Data ss:Type="Number"><xsl:value-of select="appr_people_num"/></Data></Cell>
          <Cell ss:StyleID="s302a"><Data ss:Type="Number"><xsl:value-of select="appr_db_glass"/></Data></Cell>
          <Cell ss:StyleID="s302a"><Data ss:Type="Number"><xsl:value-of select="appr_plp"/></Data></Cell>
          <Cell ss:StyleID="s302a"><Data ss:Type="Number"><xsl:value-of select="appr_object_volume"/></Data></Cell>
          <Cell ss:StyleID="s302a"><Data ss:Type="Number"><xsl:value-of select="appr_total_vol"/></Data></Cell>
          <Cell ss:StyleID="s302a"><Data ss:Type="Number"><xsl:value-of select="appr_vol_heat"/></Data></Cell>
          <Cell ss:StyleID="s302a"><Data ss:Type="Number"><xsl:value-of select="appr_vol_vent"/></Data></Cell>
          <Cell ss:StyleID="s302a"><Data ss:Type="Number"><xsl:value-of select="appr_vol_sput"/></Data></Cell>
          <Cell ss:StyleID="s302a"><Data ss:Type="Number"><xsl:value-of select="appr_vol_lost"/></Data></Cell>
          <Cell ss:StyleID="s302a"><Data ss:Type="Number"><xsl:value-of select="appr_vol_heat_cw"/></Data></Cell>
          <Cell ss:StyleID="s302a"><Data ss:Type="Number"><xsl:value-of select="appr_vol_heat_ow"/></Data></Cell>
          <Cell ss:StyleID="s302a"><Data ss:Type="Number"><xsl:value-of select="appr_vol_to"/></Data></Cell>
          <Cell ss:StyleID="s302a"><Data ss:Type="Number"><xsl:value-of select="prog_floor_num"/></Data></Cell>
          <Cell ss:StyleID="s302a"><Data ss:Type="String"><xsl:value-of select="prog_ac"/></Data></Cell>
          <Cell ss:StyleID="s302a"><Data ss:Type="Number"><xsl:value-of select="prog_people_num"/></Data></Cell>
          <Cell ss:StyleID="s302a"><Data ss:Type="Number"><xsl:value-of select="prog_db_glass"/></Data></Cell>
          <Cell ss:StyleID="s302a"><Data ss:Type="Number"><xsl:value-of select="prog_plp"/></Data></Cell>
          <Cell ss:StyleID="s302a"><Data ss:Type="Number"><xsl:value-of select="prog_object_volume"/></Data></Cell>
          <Cell ss:StyleID="s302a"><Data ss:Type="Number"><xsl:value-of select="prog_total_vol"/></Data></Cell>
          <Cell ss:StyleID="s302a"><Data ss:Type="Number"><xsl:value-of select="prog_vol_heat"/></Data></Cell>
          <Cell ss:StyleID="s302a"><Data ss:Type="Number"><xsl:value-of select="prog_vol_vent"/></Data></Cell>
          <Cell ss:StyleID="s302a"><Data ss:Type="Number"><xsl:value-of select="prog_vol_sput"/></Data></Cell>
          <Cell ss:StyleID="s302a"><Data ss:Type="Number"><xsl:value-of select="prog_vol_lost"/></Data></Cell>
          <Cell ss:StyleID="s302a"><Data ss:Type="Number"><xsl:value-of select="prog_vol_heat_cw"/></Data></Cell>
          <Cell ss:StyleID="s302a"><Data ss:Type="Number"><xsl:value-of select="prog_vol_heat_ow"/></Data></Cell>
          <Cell ss:StyleID="s302a"><Data ss:Type="Number"><xsl:value-of select="prog_vol_to"/></Data></Cell>
          <Cell ss:StyleID="s302a" ss:Formula="=RC[-14]-RC[-28]"><Data ss:Type="Number"></Data></Cell>
          <Cell ss:StyleID="s302a" ss:Formula="=RC[-14]-RC[-28]"><Data ss:Type="Number"></Data></Cell>
          <Cell ss:StyleID="s302a" ss:Formula="=RC[-14]-RC[-28]"><Data ss:Type="Number"></Data></Cell>
          <Cell ss:StyleID="s302a" ss:Formula="=RC[-14]-RC[-28]"><Data ss:Type="Number"></Data></Cell>
          <Cell ss:StyleID="s302a" ss:Formula="=RC[-14]-RC[-28]"><Data ss:Type="Number"></Data></Cell>
          <Cell ss:StyleID="s302a" ss:Formula="=RC[-14]-RC[-28]"><Data ss:Type="Number"></Data></Cell>
          <Cell ss:StyleID="s302a" ss:Formula="=RC[-14]-RC[-28]"><Data ss:Type="Number"></Data></Cell>
          <Cell ss:StyleID="s302a" ss:Formula="=RC[-14]-RC[-28]"><Data ss:Type="Number"></Data></Cell>
          <Cell ss:StyleID="s302a" ss:Formula="=RC[-14]-RC[-28]"><Data ss:Type="Number"></Data></Cell>
          <Cell ss:StyleID="s302a" ss:Formula="=RC[-14]-RC[-28]"><Data ss:Type="Number"></Data></Cell>
          <Cell ss:StyleID="s302a" ss:Formula="=RC[-14]-RC[-28]"><Data ss:Type="Number"></Data></Cell>
          <Cell ss:StyleID="s302a" ss:Formula="=RC[-14]-RC[-28]"><Data ss:Type="Number"></Data></Cell>
          <Cell ss:StyleID="s302a" ss:Formula="=RC[-14]-RC[-28]"><Data ss:Type="Number"></Data></Cell>
          <Cell ss:StyleID="s302a" ss:Formula="=RC[-14]-RC[-28]"><Data ss:Type="Number"></Data></Cell>
          <Cell ss:StyleID="s302a"><Data ss:Type="String"></Data></Cell>
        </Row>
      </xsl:when>
    </xsl:choose>
  </xsl:template>
  <!-- <<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<< Шаблон данных листа ТХ5 >>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>> -->
  <xsl:template match="TS5">
    <xsl:variable name="colCount" select="colCount"/>
    <xsl:choose>
      <xsl:when test="rowNum = 1">
        <Row>
          <Cell ss:StyleID="s40078"><Data ss:Type="String">№ п/п</Data></Cell>
          <Cell ss:StyleID="s39934"><Data ss:Type="String">Общие данные</Data></Cell>
          <Cell ss:StyleID="s39935"/>
          <xsl:call-template name="TS5PrintRow"><xsl:with-param name="pStart" select="1"/><xsl:with-param name="pEnd" select="$colCount"/><xsl:with-param name="pStyle" select="'s39935'"/><xsl:with-param name="pType" select="'String'"/></xsl:call-template>
        </Row>
      </xsl:when>
      <xsl:when test="rowNum = 2">
        <Row>
          <Cell ss:StyleID="s39910"><Data ss:Type="String">1</Data></Cell>
          <Cell ss:StyleID="s39236"><Data ss:Type="String">Период</Data></Cell>
          <Cell ss:StyleID="s39909"/>
          <xsl:call-template name="TS5PrintRow"><xsl:with-param name="pStart" select="1"/><xsl:with-param name="pEnd" select="$colCount"/><xsl:with-param name="pStyle" select="'s119'"/><xsl:with-param name="pType" select="'String'"/></xsl:call-template>
        </Row>
      </xsl:when>
      <xsl:when test="rowNum = 3">
        <Row>
          <Cell ss:StyleID="s39910"><Data ss:Type="String">2</Data></Cell>
          <Cell ss:StyleID="s39236"><Data ss:Type="String">Район (филиал)</Data></Cell>
          <Cell ss:StyleID="s39909"/>
          <xsl:call-template name="TS5PrintRow"><xsl:with-param name="pStart" select="1"/><xsl:with-param name="pEnd" select="$colCount"/><xsl:with-param name="pStyle" select="'s119'"/><xsl:with-param name="pType" select="'String'"/></xsl:call-template>
        </Row>
      </xsl:when>
      <xsl:when test="rowNum = 4">
        <Row>
          <Cell ss:StyleID="s39910"><Data ss:Type="String">3</Data></Cell>
          <Cell ss:StyleID="s39236"><Data ss:Type="String">Вид топлива</Data></Cell>
          <Cell ss:StyleID="s39909"/>
          <xsl:call-template name="TS5PrintRow"><xsl:with-param name="pStart" select="1"/><xsl:with-param name="pEnd" select="$colCount"/><xsl:with-param name="pStyle" select="'s119'"/><xsl:with-param name="pType" select="'String'"/></xsl:call-template>
        </Row>
      </xsl:when>
      <xsl:when test="rowNum = 5">
        <Row>
          <Cell ss:StyleID="s39910"><Data ss:Type="String">4</Data></Cell>
          <Cell ss:StyleID="s39911"><Data ss:Type="String">Тип объекта</Data></Cell>
          <Cell ss:StyleID="s39909"/>
          <xsl:call-template name="TS5PrintRow"><xsl:with-param name="pStart" select="1"/><xsl:with-param name="pEnd" select="$colCount"/><xsl:with-param name="pStyle" select="'s121'"/><xsl:with-param name="pType" select="'String'"/></xsl:call-template>
        </Row>
      </xsl:when>
      <xsl:when test="rowNum = 6">
        <Row>
          <Cell ss:StyleID="s39912"><Data ss:Type="String">5</Data></Cell>
          <Cell ss:StyleID="s39911"><Data ss:Type="String">Наслег</Data></Cell>
          <Cell ss:StyleID="s39911"/>
          <xsl:call-template name="TS5PrintRow"><xsl:with-param name="pStart" select="1"/><xsl:with-param name="pEnd" select="$colCount"/><xsl:with-param name="pStyle" select="'s121'"/><xsl:with-param name="pType" select="'String'"/></xsl:call-template>
        </Row>
      </xsl:when>
      <xsl:when test="rowNum = 7">
        <Row>
          <Cell ss:StyleID="s39912"><Data ss:Type="String">6</Data></Cell>
          <Cell ss:StyleID="s39911"><Data ss:Type="String">Населенный пункт</Data></Cell>
          <Cell ss:StyleID="s39911"/>
          <xsl:call-template name="TS5PrintRow"><xsl:with-param name="pStart" select="1"/><xsl:with-param name="pEnd" select="$colCount"/><xsl:with-param name="pStyle" select="'s121'"/><xsl:with-param name="pType" select="'String'"/></xsl:call-template>
        </Row>
      </xsl:when>
      <xsl:when test="rowNum = 8">
        <Row>
          <Cell ss:StyleID="s39910"><Data ss:Type="String">7</Data></Cell>
          <Cell ss:StyleID="s39909"><Data ss:Type="String">Наименование</Data></Cell>
          <Cell ss:StyleID="s39909"/>
          <xsl:call-template name="TS5PrintRow"><xsl:with-param name="pStart" select="1"/><xsl:with-param name="pEnd" select="$colCount"/><xsl:with-param name="pStyle" select="'s119'"/><xsl:with-param name="pType" select="'String'"/></xsl:call-template>
        </Row>
      </xsl:when>
      <xsl:when test="rowNum = 9">
        <Row>
          <Cell ss:StyleID="s39912"><Data ss:Type="String">8</Data></Cell>
          <Cell ss:StyleID="s39911"><Data ss:Type="String">Идентификационный код</Data></Cell>
          <Cell ss:StyleID="s39911"/>
          <xsl:call-template name="TS5PrintRow"><xsl:with-param name="pStart" select="1"/><xsl:with-param name="pEnd" select="$colCount"/><xsl:with-param name="pStyle" select="'s121'"/><xsl:with-param name="pType" select="'String'"/></xsl:call-template>
        </Row>
      </xsl:when>
      <xsl:when test="rowNum = 10">
        <Row>
          <Cell ss:StyleID="s39912"><Data ss:Type="String">9</Data></Cell>
          <Cell ss:StyleID="s39911"><Data ss:Type="String">Год ввода</Data></Cell>
          <Cell ss:StyleID="s39911"/>
          <xsl:call-template name="TS5PrintRow"><xsl:with-param name="pStart" select="1"/><xsl:with-param name="pEnd" select="$colCount"/><xsl:with-param name="pStyle" select="'s121'"/><xsl:with-param name="pType" select="'String'"/></xsl:call-template>
        </Row>
      </xsl:when>
      <xsl:when test="rowNum = 11">
        <Row>
          <Cell ss:StyleID="s39912"><Data ss:Type="String">10</Data></Cell>
          <Cell ss:StyleID="s39911"><Data ss:Type="String">Характеристика здания</Data></Cell>
          <Cell ss:StyleID="s39911"/>
          <xsl:call-template name="TS5PrintRow"><xsl:with-param name="pStart" select="1"/><xsl:with-param name="pEnd" select="$colCount"/><xsl:with-param name="pStyle" select="'s121'"/><xsl:with-param name="pType" select="'String'"/></xsl:call-template>
        </Row>
      </xsl:when>
      <xsl:when test="rowNum = 12">
        <Row>
          <Cell ss:StyleID="s40078"/>
          <Cell ss:StyleID="s39934"><Data ss:Type="String">Оборудование</Data></Cell>
          <Cell ss:StyleID="s39935"/>
          <xsl:call-template name="TS5PrintRow"><xsl:with-param name="pStart" select="1"/><xsl:with-param name="pEnd" select="$colCount"/><xsl:with-param name="pStyle" select="'s39935'"/><xsl:with-param name="pType" select="'String'"/></xsl:call-template>
        </Row>
      </xsl:when>
      <xsl:when test="rowNum = 13">
        <Row>
          <Cell ss:StyleID="s39910"><Data ss:Type="String">11</Data></Cell>
          <Cell ss:StyleID="s39909"><Data ss:Type="String">Количество котельных</Data></Cell>
          <Cell ss:StyleID="s39909"><Data ss:Type="String">ед.</Data></Cell>
          <xsl:call-template name="TS5PrintRow"><xsl:with-param name="pStart" select="1"/><xsl:with-param name="pEnd" select="$colCount"/><xsl:with-param name="pStyle" select="'s108'"/><xsl:with-param name="pType" select="'Number'"/></xsl:call-template>
        </Row>
      </xsl:when>
      <xsl:when test="rowNum = 14">
        <Row>
          <Cell ss:StyleID="s39912"><Data ss:Type="String">11.1</Data></Cell>
          <Cell ss:StyleID="s39310"><Data ss:Type="String">в т.ч. ТЭС</Data></Cell>
          <Cell ss:StyleID="s39911"><Data ss:Type="String">ед.</Data></Cell>
          <xsl:call-template name="TS5PrintRow"><xsl:with-param name="pStart" select="1"/><xsl:with-param name="pEnd" select="$colCount"/><xsl:with-param name="pStyle" select="'s113'"/><xsl:with-param name="pType" select="'Number'"/></xsl:call-template>
        </Row>
      </xsl:when>
      <xsl:when test="rowNum = 15">
        <Row>
          <Cell ss:StyleID="s39912"><Data ss:Type="String">11.2</Data></Cell>
          <Cell ss:StyleID="s39310"><Data ss:Type="String">в т.ч. котельные на органическом топливе</Data></Cell>
          <Cell ss:StyleID="s39911"><Data ss:Type="String">ед.</Data></Cell>
          <xsl:call-template name="TS5PrintRow"><xsl:with-param name="pStart" select="1"/><xsl:with-param name="pEnd" select="$colCount"/><xsl:with-param name="pStyle" select="'s113'"/><xsl:with-param name="pType" select="'Number'"/></xsl:call-template>
        </Row>
      </xsl:when>
      <xsl:when test="rowNum = 16">
        <Row>
          <Cell ss:StyleID="s39912"><Data ss:Type="String">11.3</Data></Cell>
          <Cell ss:StyleID="s39310"><Data ss:Type="String">в т.ч. электрокотельные</Data></Cell>
          <Cell ss:StyleID="s39911"><Data ss:Type="String">ед.</Data></Cell>
          <xsl:call-template name="TS5PrintRow"><xsl:with-param name="pStart" select="1"/><xsl:with-param name="pEnd" select="$colCount"/><xsl:with-param name="pStyle" select="'s113'"/><xsl:with-param name="pType" select="'Number'"/></xsl:call-template>
        </Row>
      </xsl:when>
      <xsl:when test="rowNum = 17">
        <Row>
          <Cell ss:StyleID="s39915"><Data ss:Type="String">12</Data></Cell>
          <Cell ss:StyleID="s39234"><Data ss:Type="String">Количество котлов</Data></Cell>
          <Cell ss:StyleID="s39234"><Data ss:Type="String">ед.</Data></Cell>
          <xsl:call-template name="TS5PrintRow"><xsl:with-param name="pStart" select="1"/><xsl:with-param name="pEnd" select="$colCount"/><xsl:with-param name="pStyle" select="'s108'"/><xsl:with-param name="pType" select="'Number'"/></xsl:call-template>
        </Row>
      </xsl:when>
      <xsl:when test="rowNum = 18">
        <Row>
          <Cell ss:StyleID="s39300"><Data ss:Type="String">12.1</Data></Cell>
          <Cell ss:StyleID="s39232"><Data ss:Type="String">в т.ч. водогрейные котлы на органическом топливе</Data></Cell>
          <Cell ss:StyleID="s39231"><Data ss:Type="String">ед.</Data></Cell>
          <xsl:call-template name="TS5PrintRow"><xsl:with-param name="pStart" select="1"/><xsl:with-param name="pEnd" select="$colCount"/><xsl:with-param name="pStyle" select="'s113'"/><xsl:with-param name="pType" select="'Number'"/></xsl:call-template>
        </Row>
      </xsl:when>
      <xsl:when test="rowNum = 19">
        <Row>
          <Cell ss:StyleID="s39300"><Data ss:Type="String">12.2</Data></Cell>
          <Cell ss:StyleID="s39232"><Data ss:Type="String">в т.ч. паровые котлы для комбинированной выработки</Data></Cell>
          <Cell ss:StyleID="s39231"><Data ss:Type="String">ед.</Data></Cell>
          <xsl:call-template name="TS5PrintRow"><xsl:with-param name="pStart" select="1"/><xsl:with-param name="pEnd" select="$colCount"/><xsl:with-param name="pStyle" select="'s113'"/><xsl:with-param name="pType" select="'Number'"/></xsl:call-template>
        </Row>
      </xsl:when>
      <xsl:when test="rowNum = 20">
        <Row>
          <Cell ss:StyleID="s39300"><Data ss:Type="String">12.3</Data></Cell>
          <Cell ss:StyleID="s39232"><Data ss:Type="String">в т.ч. подогреватели сетевой воды (парово-водяных, газо-водяных)</Data></Cell>
          <Cell ss:StyleID="s39231"><Data ss:Type="String">ед.</Data></Cell>
          <xsl:call-template name="TS5PrintRow"><xsl:with-param name="pStart" select="1"/><xsl:with-param name="pEnd" select="$colCount"/><xsl:with-param name="pStyle" select="'s113'"/><xsl:with-param name="pType" select="'Number'"/></xsl:call-template>
        </Row>
      </xsl:when>
      <xsl:when test="rowNum = 21">
        <Row>
          <Cell ss:StyleID="s39300"><Data ss:Type="String">12.4</Data></Cell>
          <Cell ss:StyleID="s39232"><Data ss:Type="String">в т.ч. электробойлеры</Data></Cell>
          <Cell ss:StyleID="s39231"><Data ss:Type="String">ед.</Data></Cell>
          <xsl:call-template name="TS5PrintRow"><xsl:with-param name="pStart" select="1"/><xsl:with-param name="pEnd" select="$colCount"/><xsl:with-param name="pStyle" select="'s113'"/><xsl:with-param name="pType" select="'Number'"/></xsl:call-template>
        </Row>
      </xsl:when>
      <xsl:when test="rowNum = 22">
        <Row>
          <Cell ss:StyleID="s39915"><Data ss:Type="Number">13</Data></Cell>
          <Cell ss:StyleID="s39234"><Data ss:Type="String">Количество насосов</Data></Cell>
          <Cell ss:StyleID="s39234"><Data ss:Type="String">ед.</Data></Cell>
          <xsl:call-template name="TS5PrintRow"><xsl:with-param name="pStart" select="1"/><xsl:with-param name="pEnd" select="$colCount"/><xsl:with-param name="pStyle" select="'s108'"/><xsl:with-param name="pType" select="'Number'"/></xsl:call-template>
        </Row>
      </xsl:when>
      <xsl:when test="rowNum = 23">
        <Row>
          <Cell ss:StyleID="s39300"><Data ss:Type="String">13.1</Data></Cell>
          <Cell ss:StyleID="s39232"><Data ss:Type="String">в т.ч. циркуляционных (сетевых)</Data></Cell>
          <Cell ss:StyleID="s39231"><Data ss:Type="String">ед.</Data></Cell>
          <xsl:call-template name="TS5PrintRow"><xsl:with-param name="pStart" select="1"/><xsl:with-param name="pEnd" select="$colCount"/><xsl:with-param name="pStyle" select="'s113'"/><xsl:with-param name="pType" select="'Number'"/></xsl:call-template>
        </Row>
      </xsl:when>
      <xsl:when test="rowNum = 24">
        <Row>
          <Cell ss:StyleID="s39300"><Data ss:Type="String">13.2</Data></Cell>
          <Cell ss:StyleID="s39232"><Data ss:Type="String">в т.ч. подпиточных</Data></Cell>
          <Cell ss:StyleID="s39231"><Data ss:Type="String">ед.</Data></Cell>
          <xsl:call-template name="TS5PrintRow"><xsl:with-param name="pStart" select="1"/><xsl:with-param name="pEnd" select="$colCount"/><xsl:with-param name="pStyle" select="'s113'"/><xsl:with-param name="pType" select="'Number'"/></xsl:call-template>
        </Row>
      </xsl:when>
      <xsl:when test="rowNum = 25">
        <Row>
          <Cell ss:StyleID="s39300"><Data ss:Type="String">13.3</Data></Cell>
          <Cell ss:StyleID="s39232"><Data ss:Type="String">в т.ч. других (хв, гв, подъём, перекачка и т.д.)</Data></Cell>
          <Cell ss:StyleID="s39231"><Data ss:Type="String">ед.</Data></Cell>
          <xsl:call-template name="TS5PrintRow"><xsl:with-param name="pStart" select="1"/><xsl:with-param name="pEnd" select="$colCount"/><xsl:with-param name="pStyle" select="'s113'"/><xsl:with-param name="pType" select="'Number'"/></xsl:call-template>
        </Row>
      </xsl:when>
      <xsl:when test="rowNum = 26">
        <Row>
          <Cell ss:StyleID="s39910"><Data ss:Type="String">14</Data></Cell>
          <Cell ss:StyleID="s39909"><Data ss:Type="String">Протяженность сетей</Data></Cell>
          <Cell ss:StyleID="s39909"><Data ss:Type="String">км.</Data></Cell>
          <xsl:call-template name="TS5PrintRow"><xsl:with-param name="pStart" select="1"/><xsl:with-param name="pEnd" select="$colCount"/><xsl:with-param name="pStyle" select="'s111'"/><xsl:with-param name="pType" select="'Number'"/></xsl:call-template>
        </Row>
      </xsl:when>
      <xsl:when test="rowNum = 27">
        <Row>
          <Cell ss:StyleID="s39230"><Data ss:Type="String">14.1</Data></Cell>
          <Cell ss:StyleID="s39229"><Data ss:Type="String">в т.ч. тепловых</Data></Cell>
          <Cell ss:StyleID="s39228"><Data ss:Type="String">км.</Data></Cell>
          <xsl:call-template name="TS5PrintRow"><xsl:with-param name="pStart" select="1"/><xsl:with-param name="pEnd" select="$colCount"/><xsl:with-param name="pStyle" select="'s109'"/><xsl:with-param name="pType" select="'Number'"/></xsl:call-template>
        </Row>
      </xsl:when>
      <xsl:when test="rowNum = 28">
        <Row>
          <Cell ss:StyleID="s39230"><Data ss:Type="String">14.2</Data></Cell>
          <Cell ss:StyleID="s39229"><Data ss:Type="String">в т.ч. ГВС</Data></Cell>
          <Cell ss:StyleID="s39228"><Data ss:Type="String">км.</Data></Cell>
          <xsl:call-template name="TS5PrintRow"><xsl:with-param name="pStart" select="1"/><xsl:with-param name="pEnd" select="$colCount"/><xsl:with-param name="pStyle" select="'s109'"/><xsl:with-param name="pType" select="'Number'"/></xsl:call-template>
        </Row>
      </xsl:when>
      <xsl:when test="rowNum = 29">
        <Row>
          <Cell ss:StyleID="s39230"><Data ss:Type="String">14.3</Data></Cell>
          <Cell ss:StyleID="s39229"><Data ss:Type="String">в т.ч. ХВС</Data></Cell>
          <Cell ss:StyleID="s39228"><Data ss:Type="String">км.</Data></Cell>
          <xsl:call-template name="TS5PrintRow"><xsl:with-param name="pStart" select="1"/><xsl:with-param name="pEnd" select="$colCount"/><xsl:with-param name="pStyle" select="'s109'"/><xsl:with-param name="pType" select="'Number'"/></xsl:call-template>
        </Row>
      </xsl:when>
      <xsl:when test="rowNum = 30">
        <Row>
          <Cell ss:StyleID="s39915"><Data ss:Type="Number">15</Data></Cell>
          <Cell ss:StyleID="s39234"><Data ss:Type="String">Прочее оборудование</Data></Cell>
          <Cell ss:StyleID="s39234"><Data ss:Type="String">ед.</Data></Cell>
          <xsl:call-template name="TS5PrintRow"><xsl:with-param name="pStart" select="1"/><xsl:with-param name="pEnd" select="$colCount"/><xsl:with-param name="pStyle" select="'s108'"/><xsl:with-param name="pType" select="'Number'"/></xsl:call-template>
        </Row>
      </xsl:when>
      <xsl:when test="rowNum = 31">
        <Row>
          <Cell ss:StyleID="s39915"><Data ss:Type="Number">16</Data></Cell>
          <Cell ss:StyleID="s39234"><Data ss:Type="String">Приборы учёта</Data></Cell>
          <Cell ss:StyleID="s39234"><Data ss:Type="String">ед.</Data></Cell>
          <xsl:call-template name="TS5PrintRow"><xsl:with-param name="pStart" select="1"/><xsl:with-param name="pEnd" select="$colCount"/><xsl:with-param name="pStyle" select="'s108'"/><xsl:with-param name="pType" select="'Number'"/></xsl:call-template>
        </Row>
      </xsl:when>
      <xsl:when test="rowNum = 32">
        <Row>
          <Cell ss:StyleID="s39300"><Data ss:Type="String">16.1</Data></Cell>
          <Cell ss:StyleID="s39232"><Data ss:Type="String">в т.ч. тепла</Data></Cell>
          <Cell ss:StyleID="s39231"><Data ss:Type="String">ед.</Data></Cell>
          <xsl:call-template name="TS5PrintRow"><xsl:with-param name="pStart" select="1"/><xsl:with-param name="pEnd" select="$colCount"/><xsl:with-param name="pStyle" select="'s113'"/><xsl:with-param name="pType" select="'Number'"/></xsl:call-template>
        </Row>
      </xsl:when>
      <xsl:when test="rowNum = 33">
        <Row>
          <Cell ss:StyleID="s39300"><Data ss:Type="String">16.2</Data></Cell>
          <Cell ss:StyleID="s39232"><Data ss:Type="String">в т.ч. электроэнергии</Data></Cell>
          <Cell ss:StyleID="s39231"><Data ss:Type="String">ед.</Data></Cell>
          <xsl:call-template name="TS5PrintRow"><xsl:with-param name="pStart" select="1"/><xsl:with-param name="pEnd" select="$colCount"/><xsl:with-param name="pStyle" select="'s113'"/><xsl:with-param name="pType" select="'Number'"/></xsl:call-template>
        </Row>
      </xsl:when>
      <xsl:when test="rowNum = 34">
        <Row>
          <Cell ss:StyleID="s39300"><Data ss:Type="String">16.3</Data></Cell>
          <Cell ss:StyleID="s39232"><Data ss:Type="String">в т.ч. воды</Data></Cell>
          <Cell ss:StyleID="s39231"><Data ss:Type="String">ед.</Data></Cell>
          <xsl:call-template name="TS5PrintRow"><xsl:with-param name="pStart" select="1"/><xsl:with-param name="pEnd" select="$colCount"/><xsl:with-param name="pStyle" select="'s113'"/><xsl:with-param name="pType" select="'Number'"/></xsl:call-template>
        </Row>
      </xsl:when>
      <xsl:when test="rowNum = 35">
        <Row>
          <Cell ss:StyleID="s39300"><Data ss:Type="String">16.4</Data></Cell>
          <Cell ss:StyleID="s39232"><Data ss:Type="String">в т.ч. жидкого топлива</Data></Cell>
          <Cell ss:StyleID="s39231"><Data ss:Type="String">ед.</Data></Cell>
          <xsl:call-template name="TS5PrintRow"><xsl:with-param name="pStart" select="1"/><xsl:with-param name="pEnd" select="$colCount"/><xsl:with-param name="pStyle" select="'s113'"/><xsl:with-param name="pType" select="'Number'"/></xsl:call-template>
        </Row>
      </xsl:when>
      <xsl:when test="rowNum = 36">
        <Row>
          <Cell ss:StyleID="s39300"><Data ss:Type="String">16.5</Data></Cell>
          <Cell ss:StyleID="s39232"><Data ss:Type="String">в т.ч. газообразного топлива</Data></Cell>
          <Cell ss:StyleID="s39231"><Data ss:Type="String">ед.</Data></Cell>
          <xsl:call-template name="TS5PrintRow"><xsl:with-param name="pStart" select="1"/><xsl:with-param name="pEnd" select="$colCount"/><xsl:with-param name="pStyle" select="'s113'"/><xsl:with-param name="pType" select="'Number'"/></xsl:call-template>
        </Row>
      </xsl:when>
      <xsl:when test="rowNum = 37">
        <Row>
          <Cell ss:StyleID="s39915"><Data ss:Type="Number">17</Data></Cell>
          <Cell ss:StyleID="s39234"><Data ss:Type="String">Частотные преобразователи</Data></Cell>
          <Cell ss:StyleID="s39231"><Data ss:Type="String">ед.</Data></Cell>
          <xsl:call-template name="TS5PrintRow"><xsl:with-param name="pStart" select="1"/><xsl:with-param name="pEnd" select="$colCount"/><xsl:with-param name="pStyle" select="'s108'"/><xsl:with-param name="pType" select="'Number'"/></xsl:call-template>
        </Row>
      </xsl:when>
      <xsl:when test="rowNum = 38">
        <Row>
          <Cell ss:StyleID="s39915"><Data ss:Type="Number">18</Data></Cell>
          <Cell ss:StyleID="s39234"><Data ss:Type="String">Количество тепловых пунктов</Data></Cell>
          <Cell ss:StyleID="s39234"><Data ss:Type="String">ед.</Data></Cell>
          <xsl:call-template name="TS5PrintRow"><xsl:with-param name="pStart" select="1"/><xsl:with-param name="pEnd" select="$colCount"/><xsl:with-param name="pStyle" select="'s108'"/><xsl:with-param name="pType" select="'Number'"/></xsl:call-template>
        </Row>
      </xsl:when>
      <xsl:when test="rowNum = 39">
        <Row>
          <Cell ss:StyleID="s39230"><Data ss:Type="String">18.1</Data></Cell>
          <Cell ss:StyleID="s39229"><Data ss:Type="String">Принадлежность тепловых пунктов</Data></Cell>
          <Cell ss:StyleID="s39228"/>
          <xsl:call-template name="TS5PrintRow"><xsl:with-param name="pStart" select="1"/><xsl:with-param name="pEnd" select="$colCount"/><xsl:with-param name="pStyle" select="'s113'"/><xsl:with-param name="pType" select="'Number'"/></xsl:call-template>
        </Row>
      </xsl:when>
      <xsl:when test="rowNum = 40">
        <Row>
          <Cell ss:StyleID="s40078"/>
          <Cell ss:StyleID="s39934"><Data ss:Type="String">Теплотехнические характеристики</Data></Cell>
          <Cell ss:StyleID="s39935"/>
          <xsl:call-template name="TS5PrintRow"><xsl:with-param name="pStart" select="1"/><xsl:with-param name="pEnd" select="$colCount"/><xsl:with-param name="pStyle" select="'s39935'"/><xsl:with-param name="pType" select="'String'"/></xsl:call-template>
        </Row>
      </xsl:when>
      <xsl:when test="rowNum = 41">
        <Row>
          <Cell ss:StyleID="s39915"><Data ss:Type="Number">19</Data></Cell>
          <Cell ss:StyleID="s39914"><Data ss:Type="String">Установленная мощность источников тепловой энергии</Data></Cell>
          <Cell ss:StyleID="s39914"><Data ss:Type="String">Гкал/час</Data></Cell>
          <xsl:call-template name="TS5PrintRow"><xsl:with-param name="pStart" select="1"/><xsl:with-param name="pEnd" select="$colCount"/><xsl:with-param name="pStyle" select="'s108'"/><xsl:with-param name="pType" select="'Number'"/></xsl:call-template>
        </Row>
      </xsl:when>
      <xsl:when test="rowNum = 42">
        <Row>
          <Cell ss:StyleID="s39912"><Data ss:Type="Number">20</Data></Cell>
          <Cell ss:StyleID="s39911"><Data ss:Type="String">Расчетная производительность источника тепловой энергии (среднегод.)</Data></Cell>
          <Cell ss:StyleID="s39911"><Data ss:Type="String">Гкал/час</Data></Cell>
          <xsl:call-template name="TS5PrintRow"><xsl:with-param name="pStart" select="1"/><xsl:with-param name="pEnd" select="$colCount"/><xsl:with-param name="pStyle" select="'s113'"/><xsl:with-param name="pType" select="'Number'"/></xsl:call-template>
        </Row>
      </xsl:when>
      <xsl:when test="rowNum = 43">
        <Row>
          <Cell ss:StyleID="s39910"><Data ss:Type="Number">21</Data></Cell>
          <Cell ss:StyleID="s39909"><Data ss:Type="String">Производительность источника тепловой энергии в пиковые нагрузки</Data></Cell>
          <Cell ss:StyleID="s39909"><Data ss:Type="String">Гкал/час</Data></Cell>
          <xsl:call-template name="TS5PrintRow"><xsl:with-param name="pStart" select="1"/><xsl:with-param name="pEnd" select="$colCount"/><xsl:with-param name="pStyle" select="'s108'"/><xsl:with-param name="pType" select="'Number'"/></xsl:call-template>
        </Row>
      </xsl:when>
      <xsl:when test="rowNum = 44">
        <Row>
          <Cell ss:StyleID="s39912"><Data ss:Type="Number">22</Data></Cell>
          <Cell ss:StyleID="s39911"><Data ss:Type="String">Суммарная тепловая мощность потребителей</Data></Cell>
          <Cell ss:StyleID="s39911"><Data ss:Type="String">Гкал/час</Data></Cell>
          <xsl:call-template name="TS5PrintRow"><xsl:with-param name="pStart" select="1"/><xsl:with-param name="pEnd" select="$colCount"/><xsl:with-param name="pStyle" select="'s113'"/><xsl:with-param name="pType" select="'Number'"/></xsl:call-template>
        </Row>
      </xsl:when>
      <xsl:when test="rowNum = 45">
        <Row>
          <Cell ss:StyleID="s39912"><Data ss:Type="Number">23</Data></Cell>
          <Cell ss:StyleID="s39911"><Data ss:Type="String">Коэффициент использования мощности</Data></Cell>
          <Cell ss:StyleID="s39911"><Data ss:Type="String">коэф.</Data></Cell>
          <xsl:call-template name="TS5PrintRow"><xsl:with-param name="pStart" select="1"/><xsl:with-param name="pEnd" select="$colCount"/><xsl:with-param name="pStyle" select="'s113'"/><xsl:with-param name="pType" select="'Number'"/></xsl:call-template>
        </Row>
      </xsl:when>
      <xsl:when test="rowNum = 46">
        <Row>
          <Cell ss:StyleID="s39910"><Data ss:Type="Number">24</Data></Cell>
          <Cell ss:StyleID="s39909"><Data ss:Type="String">Коэффициент использования мощности в пиковые нагрузки</Data></Cell>
          <Cell ss:StyleID="s39909"><Data ss:Type="String">коэф.</Data></Cell>
          <xsl:call-template name="TS5PrintRow"><xsl:with-param name="pStart" select="1"/><xsl:with-param name="pEnd" select="$colCount"/><xsl:with-param name="pStyle" select="'s108'"/><xsl:with-param name="pType" select="'Number'"/></xsl:call-template>
        </Row>
      </xsl:when>
      <xsl:when test="rowNum = 47">
        <Row>
          <Cell ss:StyleID="s39910"><Data ss:Type="Number">25</Data></Cell>
          <Cell ss:StyleID="s39909"><Data ss:Type="String">КПД источников тепловой энергии</Data></Cell>
          <Cell ss:StyleID="s39909"><Data ss:Type="String">коэф.</Data></Cell>
          <xsl:call-template name="TS5PrintRow"><xsl:with-param name="pStart" select="1"/><xsl:with-param name="pEnd" select="$colCount"/><xsl:with-param name="pStyle" select="'s108'"/><xsl:with-param name="pType" select="'Number'"/></xsl:call-template>
        </Row>
      </xsl:when>
      <xsl:when test="rowNum = 48">
        <Row>
          <Cell ss:StyleID="s39910"><Data ss:Type="Number">26</Data></Cell>
          <Cell ss:StyleID="s39318"><Data ss:Type="String">Реализация теплоэнергии</Data></Cell>
          <Cell ss:StyleID="s39909"><Data ss:Type="String">Гкал</Data></Cell>
          <xsl:call-template name="TS5PrintRow"><xsl:with-param name="pStart" select="1"/><xsl:with-param name="pEnd" select="$colCount"/><xsl:with-param name="pStyle" select="'s108'"/><xsl:with-param name="pType" select="'Number'"/></xsl:call-template>
        </Row>
      </xsl:when>
      <xsl:when test="rowNum = 49">
        <Row>
          <Cell ss:StyleID="s39317"><Data ss:Type="String">26.1.1</Data></Cell>
          <Cell ss:StyleID="s39316"><Data ss:Type="String">в т.ч. на отопление</Data></Cell>
          <Cell ss:StyleID="s39911"><Data ss:Type="String">Гкал</Data></Cell>
          <xsl:call-template name="TS5PrintRow"><xsl:with-param name="pStart" select="1"/><xsl:with-param name="pEnd" select="$colCount"/><xsl:with-param name="pStyle" select="'s113'"/><xsl:with-param name="pType" select="'Number'"/></xsl:call-template>
        </Row>
      </xsl:when>
      <xsl:when test="rowNum = 50">
        <Row>
          <Cell ss:StyleID="s39317"><Data ss:Type="String">26.1.2</Data></Cell>
          <Cell ss:StyleID="s39316"><Data ss:Type="String">в т.ч. на вентиляцию</Data></Cell>
          <Cell ss:StyleID="s39911"><Data ss:Type="String">Гкал</Data></Cell>
          <xsl:call-template name="TS5PrintRow"><xsl:with-param name="pStart" select="1"/><xsl:with-param name="pEnd" select="$colCount"/><xsl:with-param name="pStyle" select="'s113'"/><xsl:with-param name="pType" select="'Number'"/></xsl:call-template>
        </Row>
      </xsl:when>
      <xsl:when test="rowNum = 51">
        <Row>
          <Cell ss:StyleID="s39317"><Data ss:Type="String">26.1.3</Data></Cell>
          <Cell ss:StyleID="s39316"><Data ss:Type="String">в т.ч. на обогрев спутником ТС</Data></Cell>
          <Cell ss:StyleID="s39911"><Data ss:Type="String">Гкал</Data></Cell>
          <xsl:call-template name="TS5PrintRow"><xsl:with-param name="pStart" select="1"/><xsl:with-param name="pEnd" select="$colCount"/><xsl:with-param name="pStyle" select="'s113'"/><xsl:with-param name="pType" select="'Number'"/></xsl:call-template>
        </Row>
      </xsl:when>
      <xsl:when test="rowNum = 52">
        <Row>
          <Cell ss:StyleID="s39317"><Data ss:Type="String">26.1.4</Data></Cell>
          <Cell ss:StyleID="s39316"><Data ss:Type="String">в т.ч. на потери в абон.сетях ТС</Data></Cell>
          <Cell ss:StyleID="s39911"><Data ss:Type="String">Гкал</Data></Cell>
          <xsl:call-template name="TS5PrintRow"><xsl:with-param name="pStart" select="1"/><xsl:with-param name="pEnd" select="$colCount"/><xsl:with-param name="pStyle" select="'s113'"/><xsl:with-param name="pType" select="'Number'"/></xsl:call-template>
        </Row>
      </xsl:when>
      <xsl:when test="rowNum = 53">
        <Row>
          <Cell ss:StyleID="s39317"><Data ss:Type="String">26.1.5</Data></Cell>
          <Cell ss:StyleID="s39316"><Data ss:Type="String">в т.ч. подогрев на централизованное ГВС  </Data></Cell>
          <Cell ss:StyleID="s39911"><Data ss:Type="String">Гкал</Data></Cell>
          <xsl:call-template name="TS5PrintRow"><xsl:with-param name="pStart" select="1"/><xsl:with-param name="pEnd" select="$colCount"/><xsl:with-param name="pStyle" select="'s113'"/><xsl:with-param name="pType" select="'Number'"/></xsl:call-template>
        </Row>
      </xsl:when>
      <xsl:when test="rowNum = 54">
        <Row>
          <Cell ss:StyleID="s39317"><Data ss:Type="String">26.1.6</Data></Cell>
          <Cell ss:StyleID="s39316"><Data ss:Type="String">в т.ч. подогрев на ГВС из системы отопления</Data></Cell>
          <Cell ss:StyleID="s39911"><Data ss:Type="String">Гкал</Data></Cell>
          <xsl:call-template name="TS5PrintRow"><xsl:with-param name="pStart" select="1"/><xsl:with-param name="pEnd" select="$colCount"/><xsl:with-param name="pStyle" select="'s113'"/><xsl:with-param name="pType" select="'Number'"/></xsl:call-template>
        </Row>
      </xsl:when>
      <xsl:when test="rowNum = 55">
        <Row>
          <Cell ss:StyleID="s39317"><Data ss:Type="String">26.1.7</Data></Cell>
          <Cell ss:StyleID="s39315"><Data ss:Type="String">в т.ч. подогрев в теплообменниках</Data></Cell>
          <Cell ss:StyleID="s39911"><Data ss:Type="String">Гкал</Data></Cell>
          <xsl:call-template name="TS5PrintRow"><xsl:with-param name="pStart" select="1"/><xsl:with-param name="pEnd" select="$colCount"/><xsl:with-param name="pStyle" select="'s113'"/><xsl:with-param name="pType" select="'Number'"/></xsl:call-template>
        </Row>
      </xsl:when>
      <xsl:when test="rowNum = 56">
        <Row>
          <Cell ss:StyleID="s39314"><Data ss:Type="String">26.2.1</Data></Cell>
          <Cell ss:StyleID="s39313"><Data ss:Type="String">реализация теплоэнергии учреждениям (ЖФ)</Data></Cell>
          <Cell ss:StyleID="s39909"><Data ss:Type="String">Гкал</Data></Cell>
          <xsl:call-template name="TS5PrintRow"><xsl:with-param name="pStart" select="1"/><xsl:with-param name="pEnd" select="$colCount"/><xsl:with-param name="pStyle" select="'s108'"/><xsl:with-param name="pType" select="'Number'"/></xsl:call-template>
        </Row>
      </xsl:when>
      <xsl:when test="rowNum = 57">
        <Row>
          <Cell ss:StyleID="s39912"><Data ss:Type="String">26.2.1.1</Data></Cell>
          <Cell ss:StyleID="s39316"><Data ss:Type="String">в т.ч. на отопление</Data></Cell>
          <Cell ss:StyleID="s39911"><Data ss:Type="String">Гкал</Data></Cell>
          <xsl:call-template name="TS5PrintRow"><xsl:with-param name="pStart" select="1"/><xsl:with-param name="pEnd" select="$colCount"/><xsl:with-param name="pStyle" select="'s113'"/><xsl:with-param name="pType" select="'Number'"/></xsl:call-template>
        </Row>
      </xsl:when>
      <xsl:when test="rowNum = 58">
        <Row>
          <Cell ss:StyleID="s39912"><Data ss:Type="String">26.2.1.2</Data></Cell>
          <Cell ss:StyleID="s39316"><Data ss:Type="String">в т.ч. на вентиляцию</Data></Cell>
          <Cell ss:StyleID="s39911"><Data ss:Type="String">Гкал</Data></Cell>
          <xsl:call-template name="TS5PrintRow"><xsl:with-param name="pStart" select="1"/><xsl:with-param name="pEnd" select="$colCount"/><xsl:with-param name="pStyle" select="'s113'"/><xsl:with-param name="pType" select="'Number'"/></xsl:call-template>
        </Row>
      </xsl:when>
      <xsl:when test="rowNum = 59">
        <Row>
          <Cell ss:StyleID="s39912"><Data ss:Type="String">26.2.1.3</Data></Cell>
          <Cell ss:StyleID="s39316"><Data ss:Type="String">в т.ч. на обогрев спутником ТС</Data></Cell>
          <Cell ss:StyleID="s39911"><Data ss:Type="String">Гкал</Data></Cell>
          <xsl:call-template name="TS5PrintRow"><xsl:with-param name="pStart" select="1"/><xsl:with-param name="pEnd" select="$colCount"/><xsl:with-param name="pStyle" select="'s113'"/><xsl:with-param name="pType" select="'Number'"/></xsl:call-template>
        </Row>
      </xsl:when>
      <xsl:when test="rowNum = 60">
        <Row>
          <Cell ss:StyleID="s39912"><Data ss:Type="String">26.2.1.4</Data></Cell>
          <Cell ss:StyleID="s39316"><Data ss:Type="String">в т.ч. на потери в абон.сетях ТС</Data></Cell>
          <Cell ss:StyleID="s39911"><Data ss:Type="String">Гкал</Data></Cell>
          <xsl:call-template name="TS5PrintRow"><xsl:with-param name="pStart" select="1"/><xsl:with-param name="pEnd" select="$colCount"/><xsl:with-param name="pStyle" select="'s113'"/><xsl:with-param name="pType" select="'Number'"/></xsl:call-template>
        </Row>
      </xsl:when>
      <xsl:when test="rowNum = 61">
        <Row>
          <Cell ss:StyleID="s39912"><Data ss:Type="String">26.2.1.5</Data></Cell>
          <Cell ss:StyleID="s39316"><Data ss:Type="String">в т.ч. подогрев на централизованное ГВС  </Data></Cell>
          <Cell ss:StyleID="s39911"><Data ss:Type="String">Гкал</Data></Cell>
          <xsl:call-template name="TS5PrintRow"><xsl:with-param name="pStart" select="1"/><xsl:with-param name="pEnd" select="$colCount"/><xsl:with-param name="pStyle" select="'s113'"/><xsl:with-param name="pType" select="'Number'"/></xsl:call-template>
        </Row>
      </xsl:when>
      <xsl:when test="rowNum = 62">
        <Row>
          <Cell ss:StyleID="s39912"><Data ss:Type="String">26.2.1.6</Data></Cell>
          <Cell ss:StyleID="s39316"><Data ss:Type="String">в т.ч. подогрев на ГВС из системы отопления</Data></Cell>
          <Cell ss:StyleID="s39911"><Data ss:Type="String">Гкал</Data></Cell>
          <xsl:call-template name="TS5PrintRow"><xsl:with-param name="pStart" select="1"/><xsl:with-param name="pEnd" select="$colCount"/><xsl:with-param name="pStyle" select="'s113'"/><xsl:with-param name="pType" select="'Number'"/></xsl:call-template>
        </Row>
      </xsl:when>
      <xsl:when test="rowNum = 63">
        <Row>
          <Cell ss:StyleID="s39912"><Data ss:Type="String">26.2.1.7</Data></Cell>
          <Cell ss:StyleID="s39315"><Data ss:Type="String">в т.ч. подогрев в теплообменниках</Data></Cell>
          <Cell ss:StyleID="s39911"><Data ss:Type="String">Гкал</Data></Cell>
          <xsl:call-template name="TS5PrintRow"><xsl:with-param name="pStart" select="1"/><xsl:with-param name="pEnd" select="$colCount"/><xsl:with-param name="pStyle" select="'s113'"/><xsl:with-param name="pType" select="'Number'"/></xsl:call-template>
        </Row>
      </xsl:when>
      <xsl:when test="rowNum = 64">
        <Row>
          <Cell ss:StyleID="s39314"><Data ss:Type="String">26.3.1</Data></Cell>
          <Cell ss:StyleID="s39313"><Data ss:Type="String">реализация теплоэнергии учреждениям (МБ)</Data></Cell>
          <Cell ss:StyleID="s39909"><Data ss:Type="String">Гкал</Data></Cell>
          <xsl:call-template name="TS5PrintRow"><xsl:with-param name="pStart" select="1"/><xsl:with-param name="pEnd" select="$colCount"/><xsl:with-param name="pStyle" select="'s108'"/><xsl:with-param name="pType" select="'Number'"/></xsl:call-template>
        </Row>
      </xsl:when>
      <xsl:when test="rowNum = 65">
        <Row>
          <Cell ss:StyleID="s39912"><Data ss:Type="String">26.3.1.1</Data></Cell>
          <Cell ss:StyleID="s39316"><Data ss:Type="String">в т.ч. на отопление</Data></Cell>
          <Cell ss:StyleID="s39911"><Data ss:Type="String">Гкал</Data></Cell>
          <xsl:call-template name="TS5PrintRow"><xsl:with-param name="pStart" select="1"/><xsl:with-param name="pEnd" select="$colCount"/><xsl:with-param name="pStyle" select="'s113'"/><xsl:with-param name="pType" select="'Number'"/></xsl:call-template>
        </Row>
      </xsl:when>
      <xsl:when test="rowNum = 66">
        <Row>
          <Cell ss:StyleID="s39912"><Data ss:Type="String">26.3.1.2</Data></Cell>
          <Cell ss:StyleID="s39316"><Data ss:Type="String">в т.ч. на вентиляцию</Data></Cell>
          <Cell ss:StyleID="s39911"><Data ss:Type="String">Гкал</Data></Cell>
          <xsl:call-template name="TS5PrintRow"><xsl:with-param name="pStart" select="1"/><xsl:with-param name="pEnd" select="$colCount"/><xsl:with-param name="pStyle" select="'s113'"/><xsl:with-param name="pType" select="'Number'"/></xsl:call-template>
        </Row>
      </xsl:when>
      <xsl:when test="rowNum = 67">
        <Row>
          <Cell ss:StyleID="s39912"><Data ss:Type="String">26.3.1.3</Data></Cell>
          <Cell ss:StyleID="s39316"><Data ss:Type="String">в т.ч. на обогрев спутником ТС</Data></Cell>
          <Cell ss:StyleID="s39911"><Data ss:Type="String">Гкал</Data></Cell>
          <xsl:call-template name="TS5PrintRow"><xsl:with-param name="pStart" select="1"/><xsl:with-param name="pEnd" select="$colCount"/><xsl:with-param name="pStyle" select="'s113'"/><xsl:with-param name="pType" select="'Number'"/></xsl:call-template>
        </Row>
      </xsl:when>
      <xsl:when test="rowNum = 68">
        <Row>
          <Cell ss:StyleID="s39912"><Data ss:Type="String">26.3.1.4</Data></Cell>
          <Cell ss:StyleID="s39316"><Data ss:Type="String">в т.ч. на потери в абон.сетях ТС</Data></Cell>
          <Cell ss:StyleID="s39911"><Data ss:Type="String">Гкал</Data></Cell>
          <xsl:call-template name="TS5PrintRow"><xsl:with-param name="pStart" select="1"/><xsl:with-param name="pEnd" select="$colCount"/><xsl:with-param name="pStyle" select="'s113'"/><xsl:with-param name="pType" select="'Number'"/></xsl:call-template>
        </Row>
      </xsl:when>
      <xsl:when test="rowNum = 69">
        <Row>
          <Cell ss:StyleID="s39912"><Data ss:Type="String">26.3.1.5</Data></Cell>
          <Cell ss:StyleID="s39316"><Data ss:Type="String">в т.ч. подогрев на централизованное ГВС  </Data></Cell>
          <Cell ss:StyleID="s39911"><Data ss:Type="String">Гкал</Data></Cell>
          <xsl:call-template name="TS5PrintRow"><xsl:with-param name="pStart" select="1"/><xsl:with-param name="pEnd" select="$colCount"/><xsl:with-param name="pStyle" select="'s113'"/><xsl:with-param name="pType" select="'Number'"/></xsl:call-template>
        </Row>
      </xsl:when>
      <xsl:when test="rowNum = 70">
        <Row>
          <Cell ss:StyleID="s39912"><Data ss:Type="String">26.3.1.6</Data></Cell>
          <Cell ss:StyleID="s39316"><Data ss:Type="String">в т.ч. подогрев на ГВС из системы отопления</Data></Cell>
          <Cell ss:StyleID="s39911"><Data ss:Type="String">Гкал</Data></Cell>
          <xsl:call-template name="TS5PrintRow"><xsl:with-param name="pStart" select="1"/><xsl:with-param name="pEnd" select="$colCount"/><xsl:with-param name="pStyle" select="'s113'"/><xsl:with-param name="pType" select="'Number'"/></xsl:call-template>
        </Row>
      </xsl:when>
      <xsl:when test="rowNum = 71">
        <Row>
          <Cell ss:StyleID="s39912"><Data ss:Type="String">26.3.1.7</Data></Cell>
          <Cell ss:StyleID="s39315"><Data ss:Type="String">в т.ч. подогрев в теплообменниках</Data></Cell>
          <Cell ss:StyleID="s39911"><Data ss:Type="String">Гкал</Data></Cell>
          <xsl:call-template name="TS5PrintRow"><xsl:with-param name="pStart" select="1"/><xsl:with-param name="pEnd" select="$colCount"/><xsl:with-param name="pStyle" select="'s113'"/><xsl:with-param name="pType" select="'Number'"/></xsl:call-template>
        </Row>
      </xsl:when>
      <xsl:when test="rowNum = 72">
        <Row>
          <Cell ss:StyleID="s39314"><Data ss:Type="String">26.4.1</Data></Cell>
          <Cell ss:StyleID="s39313"><Data ss:Type="String">реализация теплоэнергии учреждениям (РБ)</Data></Cell>
          <Cell ss:StyleID="s39909"><Data ss:Type="String">Гкал</Data></Cell>
          <xsl:call-template name="TS5PrintRow"><xsl:with-param name="pStart" select="1"/><xsl:with-param name="pEnd" select="$colCount"/><xsl:with-param name="pStyle" select="'s108'"/><xsl:with-param name="pType" select="'Number'"/></xsl:call-template>
        </Row>
      </xsl:when>
      <xsl:when test="rowNum = 73">
        <Row>
          <Cell ss:StyleID="s39912"><Data ss:Type="String">26.4.1.1</Data></Cell>
          <Cell ss:StyleID="s39316"><Data ss:Type="String">в т.ч. на отопление</Data></Cell>
          <Cell ss:StyleID="s39911"><Data ss:Type="String">Гкал</Data></Cell>
          <xsl:call-template name="TS5PrintRow"><xsl:with-param name="pStart" select="1"/><xsl:with-param name="pEnd" select="$colCount"/><xsl:with-param name="pStyle" select="'s113'"/><xsl:with-param name="pType" select="'Number'"/></xsl:call-template>
        </Row>
      </xsl:when>
      <xsl:when test="rowNum = 74">
        <Row>
          <Cell ss:StyleID="s39912"><Data ss:Type="String">26.4.1.2</Data></Cell>
          <Cell ss:StyleID="s39316"><Data ss:Type="String">в т.ч. на вентиляцию</Data></Cell>
          <Cell ss:StyleID="s39911"><Data ss:Type="String">Гкал</Data></Cell>
          <xsl:call-template name="TS5PrintRow"><xsl:with-param name="pStart" select="1"/><xsl:with-param name="pEnd" select="$colCount"/><xsl:with-param name="pStyle" select="'s113'"/><xsl:with-param name="pType" select="'Number'"/></xsl:call-template>
        </Row>
      </xsl:when>
      <xsl:when test="rowNum = 75">
        <Row>
          <Cell ss:StyleID="s39912"><Data ss:Type="String">26.4.1.3</Data></Cell>
          <Cell ss:StyleID="s39316"><Data ss:Type="String">в т.ч. на обогрев спутником ТС</Data></Cell>
          <Cell ss:StyleID="s39911"><Data ss:Type="String">Гкал</Data></Cell>
          <xsl:call-template name="TS5PrintRow"><xsl:with-param name="pStart" select="1"/><xsl:with-param name="pEnd" select="$colCount"/><xsl:with-param name="pStyle" select="'s113'"/><xsl:with-param name="pType" select="'Number'"/></xsl:call-template>
        </Row>
      </xsl:when>
      <xsl:when test="rowNum = 76">
        <Row>
          <Cell ss:StyleID="s39912"><Data ss:Type="String">26.4.1.4</Data></Cell>
          <Cell ss:StyleID="s39316"><Data ss:Type="String">в т.ч. на потери в абон.сетях ТС</Data></Cell>
          <Cell ss:StyleID="s39911"><Data ss:Type="String">Гкал</Data></Cell>
          <xsl:call-template name="TS5PrintRow"><xsl:with-param name="pStart" select="1"/><xsl:with-param name="pEnd" select="$colCount"/><xsl:with-param name="pStyle" select="'s113'"/><xsl:with-param name="pType" select="'Number'"/></xsl:call-template>
        </Row>
      </xsl:when>
      <xsl:when test="rowNum = 77">
        <Row>
          <Cell ss:StyleID="s39912"><Data ss:Type="String">26.4.1.5</Data></Cell>
          <Cell ss:StyleID="s39316"><Data ss:Type="String">в т.ч. подогрев на централизованное ГВС  </Data></Cell>
          <Cell ss:StyleID="s39911"><Data ss:Type="String">Гкал</Data></Cell>
          <xsl:call-template name="TS5PrintRow"><xsl:with-param name="pStart" select="1"/><xsl:with-param name="pEnd" select="$colCount"/><xsl:with-param name="pStyle" select="'s113'"/><xsl:with-param name="pType" select="'Number'"/></xsl:call-template>
        </Row>
      </xsl:when>
      <xsl:when test="rowNum = 78">
        <Row>
          <Cell ss:StyleID="s39912"><Data ss:Type="String">26.4.1.6</Data></Cell>
          <Cell ss:StyleID="s39316"><Data ss:Type="String">в т.ч. подогрев на ГВС из системы отопления</Data></Cell>
          <Cell ss:StyleID="s39911"><Data ss:Type="String">Гкал</Data></Cell>
          <xsl:call-template name="TS5PrintRow"><xsl:with-param name="pStart" select="1"/><xsl:with-param name="pEnd" select="$colCount"/><xsl:with-param name="pStyle" select="'s113'"/><xsl:with-param name="pType" select="'Number'"/></xsl:call-template>
        </Row>
      </xsl:when>
      <xsl:when test="rowNum = 79">
        <Row>
          <Cell ss:StyleID="s39912"><Data ss:Type="String">26.4.1.7</Data></Cell>
          <Cell ss:StyleID="s39315"><Data ss:Type="String">в т.ч. подогрев в теплообменниках</Data></Cell>
          <Cell ss:StyleID="s39911"><Data ss:Type="String">Гкал</Data></Cell>
          <xsl:call-template name="TS5PrintRow"><xsl:with-param name="pStart" select="1"/><xsl:with-param name="pEnd" select="$colCount"/><xsl:with-param name="pStyle" select="'s113'"/><xsl:with-param name="pType" select="'Number'"/></xsl:call-template>
        </Row>
      </xsl:when>
      <xsl:when test="rowNum = 80">
        <Row>
          <Cell ss:StyleID="s39314"><Data ss:Type="String">26.5.1</Data></Cell>
          <Cell ss:StyleID="s39313"><Data ss:Type="String">реализация теплоэнергии учреждениям (ФБ)</Data></Cell>
          <Cell ss:StyleID="s39909"><Data ss:Type="String">Гкал</Data></Cell>
          <xsl:call-template name="TS5PrintRow"><xsl:with-param name="pStart" select="1"/><xsl:with-param name="pEnd" select="$colCount"/><xsl:with-param name="pStyle" select="'s108'"/><xsl:with-param name="pType" select="'Number'"/></xsl:call-template>
        </Row>
      </xsl:when>
      <xsl:when test="rowNum = 81">
        <Row>
          <Cell ss:StyleID="s39912"><Data ss:Type="String">26.5.1.1</Data></Cell>
          <Cell ss:StyleID="s39316"><Data ss:Type="String">в т.ч. на отопление</Data></Cell>
          <Cell ss:StyleID="s39911"><Data ss:Type="String">Гкал</Data></Cell>
          <xsl:call-template name="TS5PrintRow"><xsl:with-param name="pStart" select="1"/><xsl:with-param name="pEnd" select="$colCount"/><xsl:with-param name="pStyle" select="'s113'"/><xsl:with-param name="pType" select="'Number'"/></xsl:call-template>
        </Row>
      </xsl:when>
      <xsl:when test="rowNum = 82">
        <Row>
          <Cell ss:StyleID="s39912"><Data ss:Type="String">26.5.1.2</Data></Cell>
          <Cell ss:StyleID="s39316"><Data ss:Type="String">в т.ч. на вентиляцию</Data></Cell>
          <Cell ss:StyleID="s39911"><Data ss:Type="String">Гкал</Data></Cell>
          <xsl:call-template name="TS5PrintRow"><xsl:with-param name="pStart" select="1"/><xsl:with-param name="pEnd" select="$colCount"/><xsl:with-param name="pStyle" select="'s113'"/><xsl:with-param name="pType" select="'Number'"/></xsl:call-template>
        </Row>
      </xsl:when>
      <xsl:when test="rowNum = 83">
        <Row>
          <Cell ss:StyleID="s39912"><Data ss:Type="String">26.5.1.3</Data></Cell>
          <Cell ss:StyleID="s39316"><Data ss:Type="String">в т.ч. на обогрев спутником ТС</Data></Cell>
          <Cell ss:StyleID="s39911"><Data ss:Type="String">Гкал</Data></Cell>
          <xsl:call-template name="TS5PrintRow"><xsl:with-param name="pStart" select="1"/><xsl:with-param name="pEnd" select="$colCount"/><xsl:with-param name="pStyle" select="'s113'"/><xsl:with-param name="pType" select="'Number'"/></xsl:call-template>
        </Row>
      </xsl:when>
      <xsl:when test="rowNum = 84">
        <Row>
          <Cell ss:StyleID="s39912"><Data ss:Type="String">26.5.1.4</Data></Cell>
          <Cell ss:StyleID="s39316"><Data ss:Type="String">в т.ч. на потери в абон.сетях ТС</Data></Cell>
          <Cell ss:StyleID="s39911"><Data ss:Type="String">Гкал</Data></Cell>
          <xsl:call-template name="TS5PrintRow"><xsl:with-param name="pStart" select="1"/><xsl:with-param name="pEnd" select="$colCount"/><xsl:with-param name="pStyle" select="'s113'"/><xsl:with-param name="pType" select="'Number'"/></xsl:call-template>
        </Row>
      </xsl:when>
      <xsl:when test="rowNum = 85">
        <Row>
          <Cell ss:StyleID="s39912"><Data ss:Type="String">26.5.1.5</Data></Cell>
          <Cell ss:StyleID="s39316"><Data ss:Type="String">в т.ч. подогрев на централизованное ГВС  </Data></Cell>
          <Cell ss:StyleID="s39911"><Data ss:Type="String">Гкал</Data></Cell>
          <xsl:call-template name="TS5PrintRow"><xsl:with-param name="pStart" select="1"/><xsl:with-param name="pEnd" select="$colCount"/><xsl:with-param name="pStyle" select="'s113'"/><xsl:with-param name="pType" select="'Number'"/></xsl:call-template>
        </Row>
      </xsl:when>
      <xsl:when test="rowNum = 86">
        <Row>
          <Cell ss:StyleID="s39912"><Data ss:Type="String">26.5.1.6</Data></Cell>
          <Cell ss:StyleID="s39316"><Data ss:Type="String">в т.ч. подогрев на ГВС из системы отопления</Data></Cell>
          <Cell ss:StyleID="s39911"><Data ss:Type="String">Гкал</Data></Cell>
          <xsl:call-template name="TS5PrintRow"><xsl:with-param name="pStart" select="1"/><xsl:with-param name="pEnd" select="$colCount"/><xsl:with-param name="pStyle" select="'s113'"/><xsl:with-param name="pType" select="'Number'"/></xsl:call-template>
        </Row>
      </xsl:when>
      <xsl:when test="rowNum = 87">
        <Row>
          <Cell ss:StyleID="s39912"><Data ss:Type="String">26.5.1.7</Data></Cell>
          <Cell ss:StyleID="s39315"><Data ss:Type="String">в т.ч. подогрев в теплообменниках</Data></Cell>
          <Cell ss:StyleID="s39911"><Data ss:Type="String">Гкал</Data></Cell>
          <xsl:call-template name="TS5PrintRow"><xsl:with-param name="pStart" select="1"/><xsl:with-param name="pEnd" select="$colCount"/><xsl:with-param name="pStyle" select="'s113'"/><xsl:with-param name="pType" select="'Number'"/></xsl:call-template>
        </Row>
      </xsl:when>
      <xsl:when test="rowNum = 88">
        <Row>
          <Cell ss:StyleID="s39314"><Data ss:Type="String">26.6.1</Data></Cell>
          <Cell ss:StyleID="s39313"><Data ss:Type="String">реализация теплоэнергии прочим потребителям</Data></Cell>
          <Cell ss:StyleID="s39909"><Data ss:Type="String">Гкал</Data></Cell>
          <xsl:call-template name="TS5PrintRow"><xsl:with-param name="pStart" select="1"/><xsl:with-param name="pEnd" select="$colCount"/><xsl:with-param name="pStyle" select="'s108'"/><xsl:with-param name="pType" select="'Number'"/></xsl:call-template>
        </Row>
      </xsl:when>
      <xsl:when test="rowNum = 89">
        <Row>
          <Cell ss:StyleID="s39912"><Data ss:Type="String">26.6.1.1</Data></Cell>
          <Cell ss:StyleID="s39316"><Data ss:Type="String">в т.ч. на отопление</Data></Cell>
          <Cell ss:StyleID="s39911"><Data ss:Type="String">Гкал</Data></Cell>
          <xsl:call-template name="TS5PrintRow"><xsl:with-param name="pStart" select="1"/><xsl:with-param name="pEnd" select="$colCount"/><xsl:with-param name="pStyle" select="'s113'"/><xsl:with-param name="pType" select="'Number'"/></xsl:call-template>
        </Row>
      </xsl:when>
      <xsl:when test="rowNum = 90">
        <Row>
          <Cell ss:StyleID="s39912"><Data ss:Type="String">26.6.1.2</Data></Cell>
          <Cell ss:StyleID="s39316"><Data ss:Type="String">в т.ч. на вентиляцию</Data></Cell>
          <Cell ss:StyleID="s39911"><Data ss:Type="String">Гкал</Data></Cell>
          <xsl:call-template name="TS5PrintRow"><xsl:with-param name="pStart" select="1"/><xsl:with-param name="pEnd" select="$colCount"/><xsl:with-param name="pStyle" select="'s113'"/><xsl:with-param name="pType" select="'Number'"/></xsl:call-template>
        </Row>
      </xsl:when>
      <xsl:when test="rowNum = 91">
        <Row>
					<Cell ss:StyleID="s39912"><Data ss:Type="String">26.6.1.3</Data></Cell>
					<Cell ss:StyleID="s39316"><Data ss:Type="String">в т.ч. на обогрев спутником ТС</Data></Cell>
					<Cell ss:StyleID="s39911"><Data ss:Type="String">Гкал</Data></Cell>
          <xsl:call-template name="TS5PrintRow"><xsl:with-param name="pStart" select="1"/><xsl:with-param name="pEnd" select="$colCount"/><xsl:with-param name="pStyle" select="'s113'"/><xsl:with-param name="pType" select="'Number'"/></xsl:call-template>
        </Row>
      </xsl:when>
      <xsl:when test="rowNum = 92">
        <Row>
  				<Cell ss:StyleID="s39912"><Data ss:Type="String">26.6.1.4</Data></Cell>
	  			<Cell ss:StyleID="s39316"><Data ss:Type="String">в т.ч. на потери в абон.сетях ТС</Data></Cell>
		  		<Cell ss:StyleID="s39911"><Data ss:Type="String">Гкал</Data></Cell>
          <xsl:call-template name="TS5PrintRow"><xsl:with-param name="pStart" select="1"/><xsl:with-param name="pEnd" select="$colCount"/><xsl:with-param name="pStyle" select="'s113'"/><xsl:with-param name="pType" select="'Number'"/></xsl:call-template>
        </Row>
      </xsl:when>
      <xsl:when test="rowNum = 93">
        <Row>
  				<Cell ss:StyleID="s39912"><Data ss:Type="String">26.6.1.5</Data></Cell>
	  			<Cell ss:StyleID="s39316"><Data ss:Type="String">в т.ч. подогрев на централизованное ГВС  </Data></Cell>
		  		<Cell ss:StyleID="s39911"><Data ss:Type="String">Гкал</Data></Cell>
          <xsl:call-template name="TS5PrintRow"><xsl:with-param name="pStart" select="1"/><xsl:with-param name="pEnd" select="$colCount"/><xsl:with-param name="pStyle" select="'s113'"/><xsl:with-param name="pType" select="'Number'"/></xsl:call-template>
        </Row>
      </xsl:when>
      <xsl:when test="rowNum = 94">
        <Row>
					<Cell ss:StyleID="s39912"><Data ss:Type="String">26.6.1.6</Data></Cell>
					<Cell ss:StyleID="s39316"><Data ss:Type="String">в т.ч. подогрев на ГВС из системы отопления</Data></Cell>
					<Cell ss:StyleID="s39911"><Data ss:Type="String">Гкал</Data></Cell>
          <xsl:call-template name="TS5PrintRow"><xsl:with-param name="pStart" select="1"/><xsl:with-param name="pEnd" select="$colCount"/><xsl:with-param name="pStyle" select="'s113'"/><xsl:with-param name="pType" select="'Number'"/></xsl:call-template>
        </Row>
      </xsl:when>
      <xsl:when test="rowNum = 95">
        <Row>
					<Cell ss:StyleID="s39912"><Data ss:Type="String">26.6.1.7</Data></Cell>
					<Cell ss:StyleID="s39315"><Data ss:Type="String">в т.ч. подогрев в теплообменниках</Data></Cell>
					<Cell ss:StyleID="s39911"><Data ss:Type="String">Гкал</Data></Cell>
          <xsl:call-template name="TS5PrintRow"><xsl:with-param name="pStart" select="1"/><xsl:with-param name="pEnd" select="$colCount"/><xsl:with-param name="pStyle" select="'s113'"/><xsl:with-param name="pType" select="'Number'"/></xsl:call-template>
        </Row>
      </xsl:when>
      <xsl:when test="rowNum = 96">
        <Row>
					<Cell ss:StyleID="s39314"><Data ss:Type="String">26.7.1</Data></Cell>
					<Cell ss:StyleID="s39313"><Data ss:Type="String">реализация теплоэнергии на нужды предприятия (ВП)</Data></Cell>
					<Cell ss:StyleID="s39909"><Data ss:Type="String">Гкал</Data></Cell>
          <xsl:call-template name="TS5PrintRow"><xsl:with-param name="pStart" select="1"/><xsl:with-param name="pEnd" select="$colCount"/><xsl:with-param name="pStyle" select="'s108'"/><xsl:with-param name="pType" select="'Number'"/></xsl:call-template>
        </Row>
      </xsl:when>
      <xsl:when test="rowNum = 97">
        <Row>
					<Cell ss:StyleID="s39912"><Data ss:Type="String">26.7.1.1</Data></Cell>
					<Cell ss:StyleID="s39316"><Data ss:Type="String">в т.ч. на отопление2</Data></Cell>
					<Cell ss:StyleID="s39911"><Data ss:Type="String">Гкал</Data></Cell>
          <xsl:call-template name="TS5PrintRow"><xsl:with-param name="pStart" select="1"/><xsl:with-param name="pEnd" select="$colCount"/><xsl:with-param name="pStyle" select="'s113'"/><xsl:with-param name="pType" select="'Number'"/></xsl:call-template>
        </Row>
      </xsl:when>
      <xsl:when test="rowNum = 98">
        <Row>
					<Cell ss:StyleID="s39912"><Data ss:Type="String">26.7.1.2</Data></Cell>
					<Cell ss:StyleID="s39316"><Data ss:Type="String">в т.ч. на вентиляцию</Data></Cell>
					<Cell ss:StyleID="s39911"><Data ss:Type="String">Гкал</Data></Cell>
          <xsl:call-template name="TS5PrintRow"><xsl:with-param name="pStart" select="1"/><xsl:with-param name="pEnd" select="$colCount"/><xsl:with-param name="pStyle" select="'s113'"/><xsl:with-param name="pType" select="'Number'"/></xsl:call-template>
        </Row>
      </xsl:when>
      <xsl:when test="rowNum = 99">
        <Row>
					<Cell ss:StyleID="s39912"><Data ss:Type="String">26.7.1.3</Data></Cell>
					<Cell ss:StyleID="s39316"><Data ss:Type="String">в т.ч. на обогрев спутником ТС</Data></Cell>
					<Cell ss:StyleID="s39911"><Data ss:Type="String">Гкал</Data></Cell>
          <xsl:call-template name="TS5PrintRow"><xsl:with-param name="pStart" select="1"/><xsl:with-param name="pEnd" select="$colCount"/><xsl:with-param name="pStyle" select="'s113'"/><xsl:with-param name="pType" select="'Number'"/></xsl:call-template>
        </Row>
      </xsl:when>
      <xsl:when test="rowNum = 100">
        <Row>
          <Cell ss:StyleID="s39912"><Data ss:Type="String">26.7.1.4</Data></Cell>
          <Cell ss:StyleID="s39316"><Data ss:Type="String">в т.ч. на потери в абон.сетях ТС</Data></Cell>
          <Cell ss:StyleID="s39911"><Data ss:Type="String">Гкал</Data></Cell>
          <xsl:call-template name="TS5PrintRow"><xsl:with-param name="pStart" select="1"/><xsl:with-param name="pEnd" select="$colCount"/><xsl:with-param name="pStyle" select="'s113'"/><xsl:with-param name="pType" select="'Number'"/></xsl:call-template>
        </Row>
      </xsl:when>
      <xsl:when test="rowNum = 101">
        <Row>
          <Cell ss:StyleID="s39912"><Data ss:Type="String">26.7.1.5</Data></Cell>
          <Cell ss:StyleID="s39316"><Data ss:Type="String">в т.ч. подогрев на централизованное ГВС  </Data></Cell>
          <Cell ss:StyleID="s39911"><Data ss:Type="String">Гкал</Data></Cell>
          <xsl:call-template name="TS5PrintRow"><xsl:with-param name="pStart" select="1"/><xsl:with-param name="pEnd" select="$colCount"/><xsl:with-param name="pStyle" select="'s113'"/><xsl:with-param name="pType" select="'Number'"/></xsl:call-template>
        </Row>
      </xsl:when>
      <xsl:when test="rowNum = 102">
        <Row>
          <Cell ss:StyleID="s39912"><Data ss:Type="String">26.7.1.6</Data></Cell>
          <Cell ss:StyleID="s39316"><Data ss:Type="String">в т.ч. подогрев на ГВС из системы отопления2</Data></Cell>
          <Cell ss:StyleID="s39911"><Data ss:Type="String">Гкал</Data></Cell>
          <xsl:call-template name="TS5PrintRow"><xsl:with-param name="pStart" select="1"/><xsl:with-param name="pEnd" select="$colCount"/><xsl:with-param name="pStyle" select="'s113'"/><xsl:with-param name="pType" select="'Number'"/></xsl:call-template>
        </Row>
      </xsl:when>
      <xsl:when test="rowNum = 103">
        <Row>
          <Cell ss:StyleID="s39912"><Data ss:Type="String">26.7.1.7</Data></Cell>
          <Cell ss:StyleID="s39315"><Data ss:Type="String">в т.ч. подогрев в теплообменниках</Data></Cell>
          <Cell ss:StyleID="s39911"><Data ss:Type="String">Гкал</Data></Cell>
          <xsl:call-template name="TS5PrintRow"><xsl:with-param name="pStart" select="1"/><xsl:with-param name="pEnd" select="$colCount"/><xsl:with-param name="pStyle" select="'s113'"/><xsl:with-param name="pType" select="'Number'"/></xsl:call-template>
        </Row>
      </xsl:when>
      <xsl:when test="rowNum = 104">
        <Row>
          <Cell ss:StyleID="s39910"><Data ss:Type="Number">27</Data></Cell>
          <Cell ss:StyleID="s39312"><Data ss:Type="String">% потерь тепла в тепловых сетях</Data></Cell>
          <Cell ss:StyleID="s39909"><Data ss:Type="String">%</Data></Cell>
          <xsl:call-template name="TS5PrintRow"><xsl:with-param name="pStart" select="1"/><xsl:with-param name="pEnd" select="$colCount"/><xsl:with-param name="pStyle" select="'s108'"/><xsl:with-param name="pType" select="'Number'"/></xsl:call-template>
        </Row>
      </xsl:when>
      <xsl:when test="rowNum = 105">
        <Row>
          <Cell ss:StyleID="s39915"><Data ss:Type="Number">28</Data></Cell>
          <Cell ss:StyleID="s39311"><Data ss:Type="String">Объём потерь тепла в тепловых сетях</Data></Cell>
          <Cell ss:StyleID="s39914"><Data ss:Type="String">Гкал</Data></Cell>
          <xsl:call-template name="TS5PrintRow"><xsl:with-param name="pStart" select="1"/><xsl:with-param name="pEnd" select="$colCount"/><xsl:with-param name="pStyle" select="'s108'"/><xsl:with-param name="pType" select="'Number'"/></xsl:call-template>
        </Row>
      </xsl:when>
      <xsl:when test="rowNum = 106">
        <Row>
          <Cell ss:StyleID="s39915"><Data ss:Type="Number">29</Data></Cell>
          <Cell ss:StyleID="s39311"><Data ss:Type="String">Объем потерь на слив теплоносителя</Data></Cell>
          <Cell ss:StyleID="s39914"><Data ss:Type="String">Гкал</Data></Cell>
          <xsl:call-template name="TS5PrintRow"><xsl:with-param name="pStart" select="1"/><xsl:with-param name="pEnd" select="$colCount"/><xsl:with-param name="pStyle" select="'s108'"/><xsl:with-param name="pType" select="'Number'"/></xsl:call-template>
        </Row>
      </xsl:when>
      <xsl:when test="rowNum = 107">
        <Row>
          <Cell ss:StyleID="s39915"><Data ss:Type="Number">30</Data></Cell>
          <Cell ss:StyleID="s39311"><Data ss:Type="String">Объем потерь по ПУ</Data></Cell>
          <Cell ss:StyleID="s39914"><Data ss:Type="String">Гкал</Data></Cell>
          <xsl:call-template name="TS5PrintRow"><xsl:with-param name="pStart" select="1"/><xsl:with-param name="pEnd" select="$colCount"/><xsl:with-param name="pStyle" select="'s108'"/><xsl:with-param name="pType" select="'Number'"/></xsl:call-template>
        </Row>
      </xsl:when>
      <xsl:when test="rowNum = 108">
        <Row>
          <Cell ss:StyleID="s39912"><Data ss:Type="Number">31</Data></Cell>
          <Cell ss:StyleID="s39310"><Data ss:Type="String">Тепло на хоз. нужды предприятия</Data></Cell>
          <Cell ss:StyleID="s39911"><Data ss:Type="String">Гкал</Data></Cell>
          <xsl:call-template name="TS5PrintRow"><xsl:with-param name="pStart" select="1"/><xsl:with-param name="pEnd" select="$colCount"/><xsl:with-param name="pStyle" select="'s113'"/><xsl:with-param name="pType" select="'Number'"/></xsl:call-template>
        </Row>
      </xsl:when>
      <xsl:when test="rowNum = 109">
        <Row>
          <Cell ss:StyleID="s39910"><Data ss:Type="Number">32</Data></Cell>
          <Cell ss:StyleID="s39909"><Data ss:Type="String">Отпуск тепловой энергии в сеть</Data></Cell>
          <Cell ss:StyleID="s39909"><Data ss:Type="String">Гкал</Data></Cell>
          <xsl:call-template name="TS5PrintRow"><xsl:with-param name="pStart" select="1"/><xsl:with-param name="pEnd" select="$colCount"/><xsl:with-param name="pStyle" select="'s108'"/><xsl:with-param name="pType" select="'Number'"/></xsl:call-template>
        </Row>
      </xsl:when>
      <xsl:when test="rowNum = 110">
        <Row>
          <Cell ss:StyleID="s39912"><Data ss:Type="Number">33</Data></Cell>
          <Cell ss:StyleID="s39310"><Data ss:Type="String">Собственные (производственные) нужды источника тепла</Data></Cell>
          <Cell ss:StyleID="s39911"><Data ss:Type="String">%</Data></Cell>
          <xsl:call-template name="TS5PrintRow"><xsl:with-param name="pStart" select="1"/><xsl:with-param name="pEnd" select="$colCount"/><xsl:with-param name="pStyle" select="'s113'"/><xsl:with-param name="pType" select="'Number'"/></xsl:call-template>
        </Row>
      </xsl:when>
      <xsl:when test="rowNum = 111">
        <Row>
          <Cell ss:StyleID="s39912"><Data ss:Type="Number">34</Data></Cell>
          <Cell ss:StyleID="s39310"><Data ss:Type="String">объём собственных нужд источника тепла</Data></Cell>
          <Cell ss:StyleID="s39911"><Data ss:Type="String">Гкал</Data></Cell>
          <xsl:call-template name="TS5PrintRow"><xsl:with-param name="pStart" select="1"/><xsl:with-param name="pEnd" select="$colCount"/><xsl:with-param name="pStyle" select="'s113'"/><xsl:with-param name="pType" select="'Number'"/></xsl:call-template>
        </Row>
      </xsl:when>
      <xsl:when test="rowNum = 112">
        <Row>
          <Cell ss:StyleID="s39910"><Data ss:Type="Number">35</Data></Cell>
          <Cell ss:StyleID="s39909"><Data ss:Type="String">Покупная теплоэнергия</Data></Cell>
          <Cell ss:StyleID="s39909"><Data ss:Type="String">Гкал</Data></Cell>
          <xsl:call-template name="TS5PrintRow"><xsl:with-param name="pStart" select="1"/><xsl:with-param name="pEnd" select="$colCount"/><xsl:with-param name="pStyle" select="'s108'"/><xsl:with-param name="pType" select="'Number'"/></xsl:call-template>
        </Row>
      </xsl:when>
      <xsl:when test="rowNum = 113">
        <Row>
          <Cell ss:StyleID="s39912"><Data ss:Type="String">35.1</Data></Cell>
          <Cell ss:StyleID="s39310"><Data ss:Type="String">из тепловой сети</Data></Cell>
          <Cell ss:StyleID="s39911"><Data ss:Type="String">Гкал</Data></Cell>
          <xsl:call-template name="TS5PrintRow"><xsl:with-param name="pStart" select="1"/><xsl:with-param name="pEnd" select="$colCount"/><xsl:with-param name="pStyle" select="'s113'"/><xsl:with-param name="pType" select="'Number'"/></xsl:call-template>
        </Row>
      </xsl:when>
      <xsl:when test="rowNum = 114">
        <Row>
          <Cell ss:StyleID="s39912"><Data ss:Type="String">35.2</Data></Cell>
          <Cell ss:StyleID="s39310"><Data ss:Type="String">потери из тепловой сети</Data></Cell>
          <Cell ss:StyleID="s39911"><Data ss:Type="String">Гкал</Data></Cell>
          <xsl:call-template name="TS5PrintRow"><xsl:with-param name="pStart" select="1"/><xsl:with-param name="pEnd" select="$colCount"/><xsl:with-param name="pStyle" select="'s113'"/><xsl:with-param name="pType" select="'Number'"/></xsl:call-template>
        </Row>
      </xsl:when>
      <xsl:when test="rowNum = 115">
        <Row>
          <Cell ss:StyleID="s39912"><Data ss:Type="String">35.3</Data></Cell>
          <Cell ss:StyleID="s39309"><Data ss:Type="String">с коллекторов блок-станций (комбинированная выработка)</Data></Cell>
          <Cell ss:StyleID="s39911"><Data ss:Type="String">Гкал</Data></Cell>
          <xsl:call-template name="TS5PrintRow"><xsl:with-param name="pStart" select="1"/><xsl:with-param name="pEnd" select="$colCount"/><xsl:with-param name="pStyle" select="'s113'"/><xsl:with-param name="pType" select="'Number'"/></xsl:call-template>
        </Row>
      </xsl:when>
      <xsl:when test="rowNum = 116">
        <Row>
          <Cell ss:StyleID="s39912"><Data ss:Type="String">35.4</Data></Cell>
          <Cell ss:StyleID="s39309"><Data ss:Type="String">потери от блок-станций</Data></Cell>
          <Cell ss:StyleID="s39911"><Data ss:Type="String">Гкал</Data></Cell>
          <xsl:call-template name="TS5PrintRow"><xsl:with-param name="pStart" select="1"/><xsl:with-param name="pEnd" select="$colCount"/><xsl:with-param name="pStyle" select="'s113'"/><xsl:with-param name="pType" select="'Number'"/></xsl:call-template>
        </Row>
      </xsl:when>
      <xsl:when test="rowNum = 117">
        <Row>
          <Cell ss:StyleID="s39910"><Data ss:Type="Number">36</Data></Cell>
          <Cell ss:StyleID="s39909"><Data ss:Type="String">Выработка тепловой энергии, в т.ч.</Data></Cell>
          <Cell ss:StyleID="s39909"><Data ss:Type="String">Гкал</Data></Cell>
          <xsl:call-template name="TS5PrintRow"><xsl:with-param name="pStart" select="1"/><xsl:with-param name="pEnd" select="$colCount"/><xsl:with-param name="pStyle" select="'s108'"/><xsl:with-param name="pType" select="'Number'"/></xsl:call-template>
        </Row>
      </xsl:when>
      <xsl:when test="rowNum = 118">
        <Row>
          <Cell ss:StyleID="s39912"><Data ss:Type="Number">37</Data></Cell>
          <Cell ss:StyleID="s39308"><Data ss:Type="String">Удельный расход условного топлива на отпуск в сеть</Data></Cell>
          <Cell ss:StyleID="s39911"><Data ss:Type="String">кг.у.т./Гкал</Data></Cell>
          <xsl:call-template name="TS5PrintRow"><xsl:with-param name="pStart" select="1"/><xsl:with-param name="pEnd" select="$colCount"/><xsl:with-param name="pStyle" select="'s113'"/><xsl:with-param name="pType" select="'Number'"/></xsl:call-template>
        </Row>
      </xsl:when>
      <xsl:when test="rowNum = 119">
        <Row>
          <Cell ss:StyleID="s39912"><Data ss:Type="Number">38</Data></Cell>
          <Cell ss:StyleID="s39308"><Data ss:Type="String">Удельный расход условного топлива на реализацию тепла (без ВП)</Data></Cell>
          <Cell ss:StyleID="s39911"><Data ss:Type="String">кг.у.т./Гкал</Data></Cell>
          <xsl:call-template name="TS5PrintRow"><xsl:with-param name="pStart" select="1"/><xsl:with-param name="pEnd" select="$colCount"/><xsl:with-param name="pStyle" select="'s113'"/><xsl:with-param name="pType" select="'Number'"/></xsl:call-template>
        </Row>
      </xsl:when>
      <xsl:when test="rowNum = 120">
        <Row>
          <Cell ss:StyleID="s39910"><Data ss:Type="Number">39</Data></Cell>
          <Cell ss:StyleID="s39307"><Data ss:Type="String">ВСЕГО расход условного топлива на отпуск тепла в сеть</Data></Cell>
          <Cell ss:StyleID="s39909"><Data ss:Type="String">т.у.т.</Data></Cell>
          <xsl:call-template name="TS5PrintRow"><xsl:with-param name="pStart" select="1"/><xsl:with-param name="pEnd" select="$colCount"/><xsl:with-param name="pStyle" select="'s108'"/><xsl:with-param name="pType" select="'Number'"/></xsl:call-template>
        </Row>
      </xsl:when>
      <xsl:when test="rowNum = 121">
        <Row>
          <Cell ss:StyleID="s39912"><Data ss:Type="Number">40</Data></Cell>
          <Cell ss:StyleID="s39308"><Data ss:Type="String">Расход условного топлива (основного) на отпуск тепла в сеть</Data></Cell>
          <Cell ss:StyleID="s39911"><Data ss:Type="String">т.у.т.</Data></Cell>
          <xsl:call-template name="TS5PrintRow"><xsl:with-param name="pStart" select="1"/><xsl:with-param name="pEnd" select="$colCount"/><xsl:with-param name="pStyle" select="'s113'"/><xsl:with-param name="pType" select="'Number'"/></xsl:call-template>
        </Row>
      </xsl:when>
      <xsl:when test="rowNum = 122">
        <Row>
          <Cell ss:StyleID="s39912"><Data ss:Type="Number">41</Data></Cell>
          <Cell ss:StyleID="s39308"><Data ss:Type="String">Переводной коэффициент топлива</Data></Cell>
          <Cell ss:StyleID="s39911"><Data ss:Type="String">коэф.</Data></Cell>
          <xsl:call-template name="TS5PrintRow"><xsl:with-param name="pStart" select="1"/><xsl:with-param name="pEnd" select="$colCount"/><xsl:with-param name="pStyle" select="'s113'"/><xsl:with-param name="pType" select="'Number'"/></xsl:call-template>
        </Row>
      </xsl:when>
      <xsl:when test="rowNum = 123">
        <Row>
          <Cell ss:StyleID="s39910"><Data ss:Type="Number">42</Data></Cell>
          <Cell ss:StyleID="s39305"><Data ss:Type="String">Расход основного топлива на отпуск тепла в сеть</Data></Cell>
          <Cell ss:StyleID="s39909"><Data ss:Type="String">н.т.</Data></Cell>
          <xsl:call-template name="TS5PrintRow"><xsl:with-param name="pStart" select="1"/><xsl:with-param name="pEnd" select="$colCount"/><xsl:with-param name="pStyle" select="'s108'"/><xsl:with-param name="pType" select="'Number'"/></xsl:call-template>
        </Row>
      </xsl:when>
      <xsl:when test="rowNum = 124">
        <Row>
          <Cell ss:StyleID="s39912"><Data ss:Type="Number">43</Data></Cell>
          <Cell ss:StyleID="s39309"><Data ss:Type="String">% потерь при транспортировке топлива</Data></Cell>
          <Cell ss:StyleID="s39911"><Data ss:Type="String">%</Data></Cell>
          <xsl:call-template name="TS5PrintRow"><xsl:with-param name="pStart" select="1"/><xsl:with-param name="pEnd" select="$colCount"/><xsl:with-param name="pStyle" select="'s113'"/><xsl:with-param name="pType" select="'Number'"/></xsl:call-template>
        </Row>
      </xsl:when>
      <xsl:when test="rowNum = 125">
        <Row>
          <Cell ss:StyleID="s39912"><Data ss:Type="Number">44</Data></Cell>
          <Cell ss:StyleID="s39309"><Data ss:Type="String">объём потерь при транспортировке топлива</Data></Cell>
          <Cell ss:StyleID="s39911"><Data ss:Type="String">н.т.</Data></Cell>
          <xsl:call-template name="TS5PrintRow"><xsl:with-param name="pStart" select="1"/><xsl:with-param name="pEnd" select="$colCount"/><xsl:with-param name="pStyle" select="'s113'"/><xsl:with-param name="pType" select="'Number'"/></xsl:call-template>
        </Row>
      </xsl:when>
      <xsl:when test="rowNum = 126">
        <Row>
          <Cell ss:StyleID="s39910"><Data ss:Type="Number">45</Data></Cell>
          <Cell ss:StyleID="s39307"><Data ss:Type="String">Расход основного топлива</Data></Cell>
          <Cell ss:StyleID="s39909"><Data ss:Type="String">н.т.</Data></Cell>
          <xsl:call-template name="TS5PrintRow"><xsl:with-param name="pStart" select="1"/><xsl:with-param name="pEnd" select="$colCount"/><xsl:with-param name="pStyle" select="'s108'"/><xsl:with-param name="pType" select="'Number'"/></xsl:call-template>
        </Row>
      </xsl:when>
      <xsl:when test="rowNum = 127">
        <Row>
          <Cell ss:StyleID="s39912"><Data ss:Type="Number">46</Data></Cell>
          <Cell ss:StyleID="s39308"><Data ss:Type="String">Зольность топлива по сертификату</Data></Cell>
          <Cell ss:StyleID="s39911"><Data ss:Type="String">%</Data></Cell>
          <xsl:call-template name="TS5PrintRow"><xsl:with-param name="pStart" select="1"/><xsl:with-param name="pEnd" select="$colCount"/><xsl:with-param name="pStyle" select="'s113'"/><xsl:with-param name="pType" select="'Number'"/></xsl:call-template>
        </Row>
      </xsl:when>
      <xsl:when test="rowNum = 128">
        <Row>
          <Cell ss:StyleID="s39912"><Data ss:Type="Number">47</Data></Cell>
          <Cell ss:StyleID="s39308"><Data ss:Type="String">Выход шлака основного топлива</Data></Cell>
          <Cell ss:StyleID="s39911"><Data ss:Type="String">н.т.</Data></Cell>
          <xsl:call-template name="TS5PrintRow"><xsl:with-param name="pStart" select="1"/><xsl:with-param name="pEnd" select="$colCount"/><xsl:with-param name="pStyle" select="'s113'"/><xsl:with-param name="pType" select="'Number'"/></xsl:call-template>
        </Row>
      </xsl:when>
      <xsl:when test="rowNum = 129">
        <Row>
          <Cell ss:StyleID="s39912"><Data ss:Type="Number">48</Data></Cell>
          <Cell ss:StyleID="s39304"><Data ss:Type="String">Всего расход условного топлива на оптуск в сеть в качестве замещения на других котельных</Data></Cell>
          <Cell ss:StyleID="s39911"><Data ss:Type="String">т.у.т</Data></Cell>
          <xsl:call-template name="TS5PrintRow"><xsl:with-param name="pStart" select="1"/><xsl:with-param name="pEnd" select="$colCount"/><xsl:with-param name="pStyle" select="'s113'"/><xsl:with-param name="pType" select="'Number'"/></xsl:call-template>
        </Row>
      </xsl:when>
      <xsl:when test="rowNum = 130">
        <Row ss:Height="29.25">
          <Cell ss:StyleID="s39912"><Data ss:Type="Number">49</Data></Cell>
          <Cell ss:StyleID="s39303"><Data ss:Type="String">Всего потери при транспортировке в качестве замещения</Data></Cell>
          <Cell ss:StyleID="s39911"><Data ss:Type="String">н.т.</Data></Cell>
          <xsl:call-template name="TS5PrintRow"><xsl:with-param name="pStart" select="1"/><xsl:with-param name="pEnd" select="$colCount"/><xsl:with-param name="pStyle" select="'s113'"/><xsl:with-param name="pType" select="'Number'"/></xsl:call-template>
        </Row>
      </xsl:when>
      <xsl:when test="rowNum = 131">
        <Row>
          <Cell ss:StyleID="s39910"><Data ss:Type="Number">50</Data></Cell>
          <Cell ss:StyleID="s39302"><Data ss:Type="String">Всего расход топлива в качестве замещения на других котельных</Data></Cell>
          <Cell ss:StyleID="s39909"><Data ss:Type="String">н.т.</Data></Cell>
          <xsl:call-template name="TS5PrintRow"><xsl:with-param name="pStart" select="1"/><xsl:with-param name="pEnd" select="$colCount"/><xsl:with-param name="pStyle" select="'s108'"/><xsl:with-param name="pType" select="'Number'"/></xsl:call-template>
        </Row>
      </xsl:when>
      <xsl:when test="rowNum = 132">
        <Row>
          <Cell ss:StyleID="s39912"><Data ss:Type="Number">51</Data></Cell>
          <Cell ss:StyleID="s39303"><Data ss:Type="String">Всего выход шлака в качестве замещения</Data></Cell>
          <Cell ss:StyleID="s39911"><Data ss:Type="String">н.т.</Data></Cell>
          <xsl:call-template name="TS5PrintRow"><xsl:with-param name="pStart" select="1"/><xsl:with-param name="pEnd" select="$colCount"/><xsl:with-param name="pStyle" select="'s113'"/><xsl:with-param name="pType" select="'Number'"/></xsl:call-template>
        </Row>
      </xsl:when>
      <xsl:when test="rowNum = 133">
        <Row>
          <Cell ss:StyleID="s39915"><Data ss:Type="Number">52</Data></Cell>
          <Cell ss:StyleID="s39301"><Data ss:Type="String">Вид замещающего топлива №1</Data></Cell>
          <Cell ss:StyleID="s39914"/>
          <xsl:call-template name="TS5PrintRow"><xsl:with-param name="pStart" select="1"/><xsl:with-param name="pEnd" select="$colCount"/><xsl:with-param name="pStyle" select="'s108'"/><xsl:with-param name="pType" select="'Number'"/></xsl:call-template>
        </Row>
      </xsl:when>
      <xsl:when test="rowNum = 134">
        <Row>
          <Cell ss:StyleID="s39300"><Data ss:Type="Number">53</Data></Cell>
          <Cell ss:StyleID="s39299"><Data ss:Type="String">Расход условного топлива</Data></Cell>
          <Cell ss:StyleID="s39298"><Data ss:Type="String">т.у.т.</Data></Cell>
          <xsl:call-template name="TS5PrintRow"><xsl:with-param name="pStart" select="1"/><xsl:with-param name="pEnd" select="$colCount"/><xsl:with-param name="pStyle" select="'s113'"/><xsl:with-param name="pType" select="'Number'"/></xsl:call-template>
        </Row>
      </xsl:when>
      <xsl:when test="rowNum = 135">
        <Row>
          <Cell ss:StyleID="s39300"><Data ss:Type="Number">54</Data></Cell>
          <Cell ss:StyleID="s39316"><Data ss:Type="String">Переводной коэффициент</Data></Cell>
          <Cell ss:StyleID="s39911"/>
          <xsl:call-template name="TS5PrintRow"><xsl:with-param name="pStart" select="1"/><xsl:with-param name="pEnd" select="$colCount"/><xsl:with-param name="pStyle" select="'s113'"/><xsl:with-param name="pType" select="'Number'"/></xsl:call-template>
        </Row>
      </xsl:when>
      <xsl:when test="rowNum = 136">
        <Row>
          <Cell ss:StyleID="s39915"><Data ss:Type="Number">55</Data></Cell>
          <Cell ss:StyleID="s39296"><Data ss:Type="String">Расход топлива на отпуск тепла в сеть</Data></Cell>
          <Cell ss:StyleID="s39914"><Data ss:Type="String">н.т.</Data></Cell>
          <xsl:call-template name="TS5PrintRow"><xsl:with-param name="pStart" select="1"/><xsl:with-param name="pEnd" select="$colCount"/><xsl:with-param name="pStyle" select="'s108'"/><xsl:with-param name="pType" select="'Number'"/></xsl:call-template>
        </Row>
      </xsl:when>
      <xsl:when test="rowNum = 137">
        <Row>
          <Cell ss:StyleID="s39300"><Data ss:Type="Number">56</Data></Cell>
          <Cell ss:StyleID="s39295"><Data ss:Type="String">% потерь при транспортировке топлива</Data></Cell>
          <Cell ss:StyleID="s39298"/>
          <xsl:call-template name="TS5PrintRow"><xsl:with-param name="pStart" select="1"/><xsl:with-param name="pEnd" select="$colCount"/><xsl:with-param name="pStyle" select="'s113'"/><xsl:with-param name="pType" select="'Number'"/></xsl:call-template>
        </Row>
      </xsl:when>
      <xsl:when test="rowNum = 138">
        <Row>
          <Cell ss:StyleID="s39300"><Data ss:Type="Number">57</Data></Cell>
          <Cell ss:StyleID="s39295"><Data ss:Type="String">объём потерь при транспортировке топлива</Data></Cell>
          <Cell ss:StyleID="s39298"><Data ss:Type="String">н.т.</Data></Cell>
          <xsl:call-template name="TS5PrintRow"><xsl:with-param name="pStart" select="1"/><xsl:with-param name="pEnd" select="$colCount"/><xsl:with-param name="pStyle" select="'s113'"/><xsl:with-param name="pType" select="'Number'"/></xsl:call-template>
        </Row>
      </xsl:when>
      <xsl:when test="rowNum = 139">
        <Row>
          <Cell ss:StyleID="s39915"><Data ss:Type="Number">58</Data></Cell>
          <Cell ss:StyleID="s39296"><Data ss:Type="String">Расход замещающего топлива №1</Data></Cell>
          <Cell ss:StyleID="s39914"><Data ss:Type="String">н.т.</Data></Cell>
          <xsl:call-template name="TS5PrintRow"><xsl:with-param name="pStart" select="1"/><xsl:with-param name="pEnd" select="$colCount"/><xsl:with-param name="pStyle" select="'s108'"/><xsl:with-param name="pType" select="'Number'"/></xsl:call-template>
        </Row>
      </xsl:when>
      <xsl:when test="rowNum = 140">
        <Row>
          <Cell ss:StyleID="s39912"><Data ss:Type="Number">59</Data></Cell>
          <Cell ss:StyleID="s39294"><Data ss:Type="String">Зольность топлива по сертификату</Data></Cell>
          <Cell ss:StyleID="s39911"><Data ss:Type="String">%</Data></Cell>
          <xsl:call-template name="TS5PrintRow"><xsl:with-param name="pStart" select="1"/><xsl:with-param name="pEnd" select="$colCount"/><xsl:with-param name="pStyle" select="'s113'"/><xsl:with-param name="pType" select="'Number'"/></xsl:call-template>
        </Row>
      </xsl:when>
      <xsl:when test="rowNum = 141">
        <Row>
          <Cell ss:StyleID="s39912"><Data ss:Type="Number">60</Data></Cell>
          <Cell ss:StyleID="s39294"><Data ss:Type="String">Выход шлака</Data></Cell>
          <Cell ss:StyleID="s39911"><Data ss:Type="String">н.т.</Data></Cell>
          <xsl:call-template name="TS5PrintRow"><xsl:with-param name="pStart" select="1"/><xsl:with-param name="pEnd" select="$colCount"/><xsl:with-param name="pStyle" select="'s113'"/><xsl:with-param name="pType" select="'Number'"/></xsl:call-template>
        </Row>
      </xsl:when>
      <xsl:when test="rowNum = 142">
        <Row>
          <Cell ss:StyleID="s39910"><Data ss:Type="Number">61</Data></Cell>
          <Cell ss:StyleID="s39301"><Data ss:Type="String">Вид замещающего топлива №2</Data></Cell>
          <Cell ss:StyleID="s39914"/>
          <xsl:call-template name="TS5PrintRow"><xsl:with-param name="pStart" select="1"/><xsl:with-param name="pEnd" select="$colCount"/><xsl:with-param name="pStyle" select="'s108'"/><xsl:with-param name="pType" select="'Number'"/></xsl:call-template>
        </Row>
      </xsl:when>
      <xsl:when test="rowNum = 143">
        <Row>
          <Cell ss:StyleID="s39912"><Data ss:Type="Number">62</Data></Cell>
          <Cell ss:StyleID="s39299"><Data ss:Type="String">Расход условного топлива</Data></Cell>
          <Cell ss:StyleID="s39298"><Data ss:Type="String">т.у.т.</Data></Cell>
          <xsl:call-template name="TS5PrintRow"><xsl:with-param name="pStart" select="1"/><xsl:with-param name="pEnd" select="$colCount"/><xsl:with-param name="pStyle" select="'s113'"/><xsl:with-param name="pType" select="'Number'"/></xsl:call-template>
        </Row>
      </xsl:when>
      <xsl:when test="rowNum = 144">
        <Row>
          <Cell ss:StyleID="s39912"><Data ss:Type="Number">63</Data></Cell>
          <Cell ss:StyleID="s39316"><Data ss:Type="String">Переводной коэффициент</Data></Cell>
          <Cell ss:StyleID="s39911"/>
          <xsl:call-template name="TS5PrintRow"><xsl:with-param name="pStart" select="1"/><xsl:with-param name="pEnd" select="$colCount"/><xsl:with-param name="pStyle" select="'s113'"/><xsl:with-param name="pType" select="'Number'"/></xsl:call-template>
        </Row>
      </xsl:when>
      <xsl:when test="rowNum = 145">
        <Row>
          <Cell ss:StyleID="s39910"><Data ss:Type="Number">64</Data></Cell>
          <Cell ss:StyleID="s39296"><Data ss:Type="String">Расход топлива на отпуск тепла в сеть</Data></Cell>
          <Cell ss:StyleID="s39914"><Data ss:Type="String">н.т.</Data></Cell>
          <xsl:call-template name="TS5PrintRow"><xsl:with-param name="pStart" select="1"/><xsl:with-param name="pEnd" select="$colCount"/><xsl:with-param name="pStyle" select="'s108'"/><xsl:with-param name="pType" select="'Number'"/></xsl:call-template>
        </Row>
      </xsl:when>
      <xsl:when test="rowNum = 146">
        <Row>
          <Cell ss:StyleID="s39912"><Data ss:Type="Number">65</Data></Cell>
          <Cell ss:StyleID="s39295"><Data ss:Type="String">% потерь при транспортировке топлива</Data></Cell>
          <Cell ss:StyleID="s39298"/>
          <xsl:call-template name="TS5PrintRow"><xsl:with-param name="pStart" select="1"/><xsl:with-param name="pEnd" select="$colCount"/><xsl:with-param name="pStyle" select="'s113'"/><xsl:with-param name="pType" select="'Number'"/></xsl:call-template>
        </Row>
      </xsl:when>
      <xsl:when test="rowNum = 147">
        <Row>
          <Cell ss:StyleID="s39912"><Data ss:Type="Number">66</Data></Cell>
          <Cell ss:StyleID="s39295"><Data ss:Type="String">объём потерь при транспортировке топлива</Data></Cell>
          <Cell ss:StyleID="s39298"><Data ss:Type="String">н.т.</Data></Cell>
          <xsl:call-template name="TS5PrintRow"><xsl:with-param name="pStart" select="1"/><xsl:with-param name="pEnd" select="$colCount"/><xsl:with-param name="pStyle" select="'s113'"/><xsl:with-param name="pType" select="'Number'"/></xsl:call-template>
        </Row>
      </xsl:when>
      <xsl:when test="rowNum = 148">
        <Row>
          <Cell ss:StyleID="s39910"><Data ss:Type="Number">67</Data></Cell>
          <Cell ss:StyleID="s39296"><Data ss:Type="String">Расход замещающего топлива №2</Data></Cell>
          <Cell ss:StyleID="s39914"><Data ss:Type="String">н.т.</Data></Cell>
          <xsl:call-template name="TS5PrintRow"><xsl:with-param name="pStart" select="1"/><xsl:with-param name="pEnd" select="$colCount"/><xsl:with-param name="pStyle" select="'s108'"/><xsl:with-param name="pType" select="'Number'"/></xsl:call-template>
        </Row>
      </xsl:when>
      <xsl:when test="rowNum = 149">
        <Row>
          <Cell ss:StyleID="s39912"><Data ss:Type="Number">68</Data></Cell>
          <Cell ss:StyleID="s39294"><Data ss:Type="String">Зольность топлива по сертификату</Data></Cell>
          <Cell ss:StyleID="s39911"><Data ss:Type="String">%</Data></Cell>
          <xsl:call-template name="TS5PrintRow"><xsl:with-param name="pStart" select="1"/><xsl:with-param name="pEnd" select="$colCount"/><xsl:with-param name="pStyle" select="'s113'"/><xsl:with-param name="pType" select="'Number'"/></xsl:call-template>
        </Row>
      </xsl:when>
      <xsl:when test="rowNum = 150">
        <Row>
          <Cell ss:StyleID="s39912"><Data ss:Type="Number">69</Data></Cell>
          <Cell ss:StyleID="s39294"><Data ss:Type="String">Выход шлака</Data></Cell>
          <Cell ss:StyleID="s39911"><Data ss:Type="String">н.т.</Data></Cell>
          <xsl:call-template name="TS5PrintRow"><xsl:with-param name="pStart" select="1"/><xsl:with-param name="pEnd" select="$colCount"/><xsl:with-param name="pStyle" select="'s113'"/><xsl:with-param name="pType" select="'Number'"/></xsl:call-template>
        </Row>
      </xsl:when>
      <xsl:when test="rowNum = 151">
        <Row>
          <Cell ss:StyleID="s39910"><Data ss:Type="Number">70</Data></Cell>
          <Cell ss:StyleID="s39301"><Data ss:Type="String">Вид замещающего топлива №1</Data></Cell>
          <Cell ss:StyleID="s39914"/>
          <xsl:call-template name="TS5PrintRow"><xsl:with-param name="pStart" select="1"/><xsl:with-param name="pEnd" select="$colCount"/><xsl:with-param name="pStyle" select="'s108'"/><xsl:with-param name="pType" select="'Number'"/></xsl:call-template>
        </Row>
      </xsl:when>
      <xsl:when test="rowNum = 152">
        <Row>
          <Cell ss:StyleID="s39912"><Data ss:Type="Number">71</Data></Cell>
          <Cell ss:StyleID="s39299"><Data ss:Type="String">Расход условного топлива</Data></Cell>
          <Cell ss:StyleID="s39298"><Data ss:Type="String">т.у.т.</Data></Cell>
          <xsl:call-template name="TS5PrintRow"><xsl:with-param name="pStart" select="1"/><xsl:with-param name="pEnd" select="$colCount"/><xsl:with-param name="pStyle" select="'s113'"/><xsl:with-param name="pType" select="'Number'"/></xsl:call-template>
        </Row>
      </xsl:when>
      <xsl:when test="rowNum = 153">
        <Row>
          <Cell ss:StyleID="s39912"><Data ss:Type="Number">72</Data></Cell>
          <Cell ss:StyleID="s39316"><Data ss:Type="String">Переводной коэффициент</Data></Cell>
          <Cell ss:StyleID="s39911"/>
          <xsl:call-template name="TS5PrintRow"><xsl:with-param name="pStart" select="1"/><xsl:with-param name="pEnd" select="$colCount"/><xsl:with-param name="pStyle" select="'s113'"/><xsl:with-param name="pType" select="'Number'"/></xsl:call-template>
        </Row>
      </xsl:when>
      <xsl:when test="rowNum = 154">
        <Row>
          <Cell ss:StyleID="s39910"><Data ss:Type="Number">73</Data></Cell>
          <Cell ss:StyleID="s39296"><Data ss:Type="String">Расход топлива на отпуск тепла в сеть</Data></Cell>
          <Cell ss:StyleID="s39914"><Data ss:Type="String">н.т.</Data></Cell>
          <xsl:call-template name="TS5PrintRow"><xsl:with-param name="pStart" select="1"/><xsl:with-param name="pEnd" select="$colCount"/><xsl:with-param name="pStyle" select="'s108'"/><xsl:with-param name="pType" select="'Number'"/></xsl:call-template>
        </Row>
      </xsl:when>
      <xsl:when test="rowNum = 155">
        <Row>
          <Cell ss:StyleID="s39912"><Data ss:Type="Number">74</Data></Cell>
          <Cell ss:StyleID="s39295"><Data ss:Type="String">% потерь при транспортировке топлива</Data></Cell>
          <Cell ss:StyleID="s39298"/>
          <xsl:call-template name="TS5PrintRow"><xsl:with-param name="pStart" select="1"/><xsl:with-param name="pEnd" select="$colCount"/><xsl:with-param name="pStyle" select="'s113'"/><xsl:with-param name="pType" select="'Number'"/></xsl:call-template>
        </Row>
      </xsl:when>
      <xsl:when test="rowNum = 156">
        <Row>
          <Cell ss:StyleID="s39912"><Data ss:Type="Number">75</Data></Cell>
          <Cell ss:StyleID="s39295"><Data ss:Type="String">объём потерь при транспортировке топлива</Data></Cell>
          <Cell ss:StyleID="s39298"><Data ss:Type="String">н.т.</Data></Cell>
          <xsl:call-template name="TS5PrintRow"><xsl:with-param name="pStart" select="1"/><xsl:with-param name="pEnd" select="$colCount"/><xsl:with-param name="pStyle" select="'s113'"/><xsl:with-param name="pType" select="'Number'"/></xsl:call-template>
        </Row>
      </xsl:when>
      <xsl:when test="rowNum = 157">
        <Row>
          <Cell ss:StyleID="s39910"><Data ss:Type="Number">76</Data></Cell>
          <Cell ss:StyleID="s39296"><Data ss:Type="String">Расход замещающего топлива №3</Data></Cell>
          <Cell ss:StyleID="s39914"><Data ss:Type="String">н.т.</Data></Cell>
          <xsl:call-template name="TS5PrintRow"><xsl:with-param name="pStart" select="1"/><xsl:with-param name="pEnd" select="$colCount"/><xsl:with-param name="pStyle" select="'s108'"/><xsl:with-param name="pType" select="'Number'"/></xsl:call-template>
        </Row>
      </xsl:when>
      <xsl:when test="rowNum = 158">
        <Row>
          <Cell ss:StyleID="s39912"><Data ss:Type="Number">77</Data></Cell>
          <Cell ss:StyleID="s39294"><Data ss:Type="String">Зольность топлива по сертификату</Data></Cell>
          <Cell ss:StyleID="s39911"><Data ss:Type="String">%</Data></Cell>
          <xsl:call-template name="TS5PrintRow"><xsl:with-param name="pStart" select="1"/><xsl:with-param name="pEnd" select="$colCount"/><xsl:with-param name="pStyle" select="'s113'"/><xsl:with-param name="pType" select="'Number'"/></xsl:call-template>
        </Row>
      </xsl:when>
      <xsl:when test="rowNum = 159">
        <Row>
          <Cell ss:StyleID="s39912"><Data ss:Type="Number">78</Data></Cell>
          <Cell ss:StyleID="s39294"><Data ss:Type="String">Выход шлака</Data></Cell>
          <Cell ss:StyleID="s39911"><Data ss:Type="String">н.т.</Data></Cell>
          <xsl:call-template name="TS5PrintRow"><xsl:with-param name="pStart" select="1"/><xsl:with-param name="pEnd" select="$colCount"/><xsl:with-param name="pStyle" select="'s113'"/><xsl:with-param name="pType" select="'Number'"/></xsl:call-template>
        </Row>
      </xsl:when>
      <xsl:when test="rowNum = 160">
        <Row>
          <Cell ss:StyleID="s39910"><Data ss:Type="Number">79</Data></Cell>
          <Cell ss:StyleID="s39307"><Data ss:Type="String">Норма расхода электроэнергии на 1 Гкал выработки</Data></Cell>
          <Cell ss:StyleID="s39909"><Data ss:Type="String">кВт*ч/Гкал</Data></Cell>
          <xsl:call-template name="TS5PrintRow"><xsl:with-param name="pStart" select="1"/><xsl:with-param name="pEnd" select="$colCount"/><xsl:with-param name="pStyle" select="'s108'"/><xsl:with-param name="pType" select="'Number'"/></xsl:call-template>
        </Row>
      </xsl:when>
      <xsl:when test="rowNum = 161">
        <Row>
          <Cell ss:StyleID="s39910"><Data ss:Type="Number">80</Data></Cell>
          <Cell ss:StyleID="s39909"><Data ss:Type="String">Расход электроэнергии</Data></Cell>
          <Cell ss:StyleID="s39909"><Data ss:Type="String">тыс. кВт*ч</Data></Cell>
          <xsl:call-template name="TS5PrintRow"><xsl:with-param name="pStart" select="1"/><xsl:with-param name="pEnd" select="$colCount"/><xsl:with-param name="pStyle" select="'s108'"/><xsl:with-param name="pType" select="'Number'"/></xsl:call-template>
        </Row>
      </xsl:when>
      <xsl:when test="rowNum = 162">
        <Row>
          <Cell ss:StyleID="s39912"><Data ss:Type="String">80.2</Data></Cell>
          <Cell ss:StyleID="s39310"><Data ss:Type="String">СН 2 (1-20 кВ) объем энергии</Data></Cell>
          <Cell ss:StyleID="s39911"><Data ss:Type="String">тыс. кВт*ч</Data></Cell>
          <xsl:call-template name="TS5PrintRow"><xsl:with-param name="pStart" select="1"/><xsl:with-param name="pEnd" select="$colCount"/><xsl:with-param name="pStyle" select="'s113'"/><xsl:with-param name="pType" select="'Number'"/></xsl:call-template>
        </Row>
      </xsl:when>
      <xsl:when test="rowNum = 163">
        <Row>
          <Cell ss:StyleID="s39912"><Data ss:Type="String">80.3</Data></Cell>
          <Cell ss:StyleID="s39310"><Data ss:Type="String">СН 1 (35 кВ) объем энергии</Data></Cell>
          <Cell ss:StyleID="s39911"><Data ss:Type="String">тыс. кВт*ч</Data></Cell>
          <xsl:call-template name="TS5PrintRow"><xsl:with-param name="pStart" select="1"/><xsl:with-param name="pEnd" select="$colCount"/><xsl:with-param name="pStyle" select="'s113'"/><xsl:with-param name="pType" select="'Number'"/></xsl:call-template>
        </Row>
      </xsl:when>
      <xsl:when test="rowNum = 164">
        <Row>
          <Cell ss:StyleID="s39912"><Data ss:Type="String">80.4</Data></Cell>
          <Cell ss:StyleID="s39309"><Data ss:Type="String">ВН (110 кВ и выше) объем энергии</Data></Cell>
          <Cell ss:StyleID="s39911"><Data ss:Type="String">тыс. кВт*ч</Data></Cell>
          <xsl:call-template name="TS5PrintRow"><xsl:with-param name="pStart" select="1"/><xsl:with-param name="pEnd" select="$colCount"/><xsl:with-param name="pStyle" select="'s113'"/><xsl:with-param name="pType" select="'Number'"/></xsl:call-template>
        </Row>
      </xsl:when>
      <xsl:when test="rowNum = 165">
        <Row>
          <Cell ss:StyleID="s39910"><Data ss:Type="Number">81</Data></Cell>
          <Cell ss:StyleID="s39909"><Data ss:Type="String">Норма расхода воды на 1 Гкал выработки</Data></Cell>
          <Cell ss:StyleID="s39909"><Data ss:Type="String">м³/Гкал</Data></Cell>
          <xsl:call-template name="TS5PrintRow"><xsl:with-param name="pStart" select="1"/><xsl:with-param name="pEnd" select="$colCount"/><xsl:with-param name="pStyle" select="'s108'"/><xsl:with-param name="pType" select="'Number'"/></xsl:call-template>
        </Row>
      </xsl:when>
      <xsl:when test="rowNum = 166">
        <Row>
          <Cell ss:StyleID="s39910"><Data ss:Type="Number">82</Data></Cell>
          <Cell ss:StyleID="s39307"><Data ss:Type="String">Расход воды на технологические нужды</Data></Cell>
          <Cell ss:StyleID="s39909"><Data ss:Type="String">м³</Data></Cell>
          <xsl:call-template name="TS5PrintRow"><xsl:with-param name="pStart" select="1"/><xsl:with-param name="pEnd" select="$colCount"/><xsl:with-param name="pStyle" select="'s108'"/><xsl:with-param name="pType" select="'Number'"/></xsl:call-template>
        </Row>
      </xsl:when>
      <xsl:when test="rowNum = 167">
        <Row>
          <Cell ss:StyleID="s39912"><Data ss:Type="String">82.1</Data></Cell>
          <Cell ss:StyleID="s39309"><Data ss:Type="String">централизованная</Data></Cell>
          <Cell ss:StyleID="s39911"><Data ss:Type="String">м³</Data></Cell>
          <xsl:call-template name="TS5PrintRow"><xsl:with-param name="pStart" select="1"/><xsl:with-param name="pEnd" select="$colCount"/><xsl:with-param name="pStyle" select="'s113'"/><xsl:with-param name="pType" select="'Number'"/></xsl:call-template>
        </Row>
      </xsl:when>
      <xsl:when test="rowNum = 168">
        <Row>
          <Cell ss:StyleID="s39912"><Data ss:Type="String">82.2</Data></Cell>
          <Cell ss:StyleID="s39309"><Data ss:Type="String">водозабор (скважина)</Data></Cell>
          <Cell ss:StyleID="s39911"><Data ss:Type="String">м³</Data></Cell>
          <xsl:call-template name="TS5PrintRow"><xsl:with-param name="pStart" select="1"/><xsl:with-param name="pEnd" select="$colCount"/><xsl:with-param name="pStyle" select="'s113'"/><xsl:with-param name="pType" select="'Number'"/></xsl:call-template>
        </Row>
      </xsl:when>
      <xsl:when test="rowNum = 169">
        <Row>
          <Cell ss:StyleID="s39912"><Data ss:Type="String">82.3</Data></Cell>
          <Cell ss:StyleID="s39309"><Data ss:Type="String">водозабор (скважина) с подвозом</Data></Cell>
          <Cell ss:StyleID="s39911"><Data ss:Type="String">м³</Data></Cell>
          <xsl:call-template name="TS5PrintRow"><xsl:with-param name="pStart" select="1"/><xsl:with-param name="pEnd" select="$colCount"/><xsl:with-param name="pStyle" select="'s113'"/><xsl:with-param name="pType" select="'Number'"/></xsl:call-template>
        </Row>
      </xsl:when>
      <xsl:when test="rowNum = 170">
        <Row>
          <Cell ss:StyleID="s39912"><Data ss:Type="String">82.4</Data></Cell>
          <Cell ss:StyleID="s39309"><Data ss:Type="String">подвозная</Data></Cell>
          <Cell ss:StyleID="s39911"><Data ss:Type="String">м³</Data></Cell>
          <xsl:call-template name="TS5PrintRow"><xsl:with-param name="pStart" select="1"/><xsl:with-param name="pEnd" select="$colCount"/><xsl:with-param name="pStyle" select="'s113'"/><xsl:with-param name="pType" select="'Number'"/></xsl:call-template>
        </Row>
      </xsl:when>
      <xsl:when test="rowNum = 171">
        <Row>
          <Cell ss:StyleID="s39910"><Data ss:Type="Number">83</Data></Cell>
          <Cell ss:StyleID="s39909"><Data ss:Type="String">Коэффициент потерь в сетях</Data></Cell>
          <Cell ss:StyleID="s39909"><Data ss:Type="String">Гкал/км</Data></Cell>
          <xsl:call-template name="TS5PrintRow"><xsl:with-param name="pStart" select="1"/><xsl:with-param name="pEnd" select="$colCount"/><xsl:with-param name="pStyle" select="'s108'"/><xsl:with-param name="pType" select="'Number'"/></xsl:call-template>
        </Row>
      </xsl:when>
      <xsl:when test="rowNum = 172">
        <Row>
          <Cell ss:StyleID="s39910"><Data ss:Type="Number">84</Data></Cell>
          <Cell ss:StyleID="s39909"><Data ss:Type="String">Численность персонала</Data></Cell>
          <Cell ss:StyleID="s39909"><Data ss:Type="String">чел.</Data></Cell>
          <xsl:call-template name="TS5PrintRow"><xsl:with-param name="pStart" select="1"/><xsl:with-param name="pEnd" select="$colCount"/><xsl:with-param name="pStyle" select="'s108'"/><xsl:with-param name="pType" select="'Number'"/></xsl:call-template>
        </Row>
      </xsl:when>
      <xsl:when test="rowNum = 173">
        <Row>
          <Cell ss:StyleID="s39240"><Data ss:Type="Number">85</Data></Cell>
          <Cell ss:StyleID="s39239"><Data ss:Type="String">Производительность труда</Data></Cell>
          <Cell ss:StyleID="s39239"><Data ss:Type="String">Гкал/чел</Data></Cell>
          <xsl:call-template name="TS5PrintRow"><xsl:with-param name="pStart" select="1"/><xsl:with-param name="pEnd" select="$colCount"/><xsl:with-param name="pStyle" select="'s108b'"/><xsl:with-param name="pType" select="'Number'"/></xsl:call-template>
        </Row>
      </xsl:when>
    </xsl:choose>
  </xsl:template>
  <!-- <<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<< Шаблон печати столбцов ТХ5 >>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>> -->
  <xsl:template name="TS5PrintColumns">
    <xsl:param name="pStart"/>
    <xsl:param name="pEnd"/>
    <xsl:param name="pWidth"/>
    <xsl:if test="not($pStart > $pEnd)">
      <xsl:choose>
        <xsl:when test="$pStart = $pEnd">
          <Column ss:AutoFitWidth="0">
            <xsl:attribute name="ss:Width"><xsl:value-of select="$pWidth"/></xsl:attribute>
          </Column>
        </xsl:when>
        <xsl:otherwise>
          <xsl:variable name="vMid" select="floor(($pStart + $pEnd) div 2)"/>
          <xsl:call-template name="TS5PrintColumns">
            <xsl:with-param name="pStart" select="$pStart"/>
            <xsl:with-param name="pEnd" select="$vMid"/>
            <xsl:with-param name="pWidth" select="$pWidth"/>
          </xsl:call-template>
          <xsl:call-template name="TS5PrintColumns">
            <xsl:with-param name="pStart" select="$vMid + 1"/>
            <xsl:with-param name="pEnd" select="$pEnd"/>
            <xsl:with-param name="pWidth" select="$pWidth"/>
          </xsl:call-template>
        </xsl:otherwise>
      </xsl:choose>
    </xsl:if>
  </xsl:template>
  <!-- <<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<< Шаблон печати строки ТХ5 >>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>> -->
  <xsl:template name="TS5PrintRow">
    <xsl:param name="pStart"/>
    <xsl:param name="pEnd"/>
    <xsl:param name="pStyle"/>
    <xsl:param name="pType"/>
    <xsl:if test="not($pStart > $pEnd)">
      <xsl:choose>
        <xsl:when test="$pStart = $pEnd">
          <Cell>
            <xsl:attribute name="ss:StyleID"><xsl:value-of select="$pStyle"/></xsl:attribute>
            <Data>
              <xsl:attribute name="ss:Type"><xsl:value-of select="$pType"/></xsl:attribute>
              <xsl:value-of select="*[name() = concat('column', $pStart)]"/>
            </Data>
          </Cell>
        </xsl:when>
        <xsl:otherwise>
          <xsl:variable name="vMid" select="floor(($pStart + $pEnd) div 2)"/>
          <xsl:call-template name="TS5PrintRow">
            <xsl:with-param name="pStart" select="$pStart"/>
            <xsl:with-param name="pEnd" select="$vMid"/>
            <xsl:with-param name="pStyle" select="$pStyle"/>
            <xsl:with-param name="pType" select="$pType"/>
          </xsl:call-template>
          <xsl:call-template name="TS5PrintRow">
            <xsl:with-param name="pStart" select="$vMid + 1"/>
            <xsl:with-param name="pEnd" select="$pEnd"/>
            <xsl:with-param name="pStyle" select="$pStyle"/>
            <xsl:with-param name="pType" select="$pType"/>
          </xsl:call-template>
        </xsl:otherwise>
      </xsl:choose>
    </xsl:if>
  </xsl:template>
  <!-- <<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<< Шаблон данных листа ТХ6 >>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>> -->
  <xsl:template match="TS6">
    <xsl:choose>
      <xsl:when test="style_id = 1">
        <!-- <<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<< Строка с улусом >>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>> -->
        <Row>
          <Cell ss:MergeAcross="5" ss:StyleID="TS6Cell1"><Data ss:Type="String"><xsl:value-of select="reg" /></Data></Cell>
          <Cell ss:StyleID="TS6Cell1"><Data ss:Type="Number"><xsl:value-of select="pow" /></Data></Cell>
          <Cell ss:StyleID="TS6Cell1"><Data ss:Type="Number"><xsl:value-of select="kpd" /></Data></Cell>
          <Cell ss:StyleID="TS6Cell1"><Data ss:Type="Number"><xsl:value-of select="Vh" /></Data></Cell>
          <Cell ss:StyleID="TS6Cell1"><Data ss:Type="Number"><xsl:value-of select="ah" /></Data></Cell>
          <Cell ss:StyleID="TS6Cell1"><Data ss:Type="Number"><xsl:value-of select="Ph" /></Data></Cell>
          <Cell ss:StyleID="TS6Cell1"><Data ss:Type="Number"><xsl:value-of select="Ak" /></Data></Cell>
          <Cell ss:StyleID="TS6Cell1"><Data ss:Type="Number"><xsl:value-of select="Pk" /></Data></Cell>
          <Cell ss:MergeAcross="1" ss:StyleID="TS6Cell2" />
        </Row>
      </xsl:when>
      <xsl:when test="style_id = 2">
        <!-- <<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<< Строка с населенным пунктом >>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>> -->
        <Row>
          <Cell ss:MergeAcross="14" ss:StyleID="TS6Cell3"><Data ss:Type="String"><xsl:value-of select="reg" /></Data></Cell>
        </Row>
      </xsl:when>
      <xsl:when test="style_id = 3">
        <!-- <<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<< Строка с котельной >>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>> -->
        <Row>
          <Cell ss:MergeAcross="5" ss:StyleID="TS6Cell4"><Data ss:Type="String"><xsl:value-of select="reg" /></Data></Cell>
          <Cell ss:StyleID="TS6Cell1"><Data ss:Type="Number"><xsl:value-of select="pow" /></Data></Cell>
          <Cell ss:StyleID="TS6Cell1"><Data ss:Type="Number"><xsl:value-of select="kpd" /></Data></Cell>
          <Cell ss:StyleID="TS6Cell1"><Data ss:Type="Number"><xsl:value-of select="Vh" /></Data></Cell>
          <Cell ss:StyleID="TS6Cell1"><Data ss:Type="Number"><xsl:value-of select="ah" /></Data></Cell>
          <Cell ss:StyleID="TS6Cell1"><Data ss:Type="Number"><xsl:value-of select="Ph" /></Data></Cell>
          <Cell ss:StyleID="TS6Cell1"><Data ss:Type="Number"><xsl:value-of select="Ak" /></Data></Cell>
          <Cell ss:StyleID="TS6Cell1"><Data ss:Type="Number"><xsl:value-of select="Pk" /></Data></Cell>
          <Cell ss:MergeAcross="1" ss:StyleID="TS6Cell2" />
        </Row>
      </xsl:when>
      <xsl:when test="style_id = 4">
        <!-- <<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<< Строка с котлами >>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>> -->
        <Row>
          <Cell ss:StyleID="TS6Cell5"><Data ss:Type="String"><xsl:value-of select="reg" /></Data></Cell>
          <Cell ss:StyleID="TS6Cell6"><Data ss:Type="String"><xsl:value-of select="th" /></Data></Cell>
          <Cell ss:StyleID="TS6Cell6"><Data ss:Type="String"><xsl:value-of select="ba" /></Data></Cell>
          <Cell ss:StyleID="TS6Cell6"><Data ss:Type="String"><xsl:value-of select="bf" /></Data></Cell>
          <Cell ss:StyleID="TS6Cell6"><Data ss:Type="String"><xsl:value-of select="ff" /></Data></Cell>
          <Cell ss:StyleID="TS6Cell6" />
          <Cell ss:StyleID="TS6Cell6"><Data ss:Type="Number"><xsl:value-of select="pow" /></Data></Cell>
          <Cell ss:StyleID="TS6Cell6"><Data ss:Type="Number"><xsl:value-of select="kpd" /></Data></Cell>
          <Cell ss:MergeAcross="4" ss:StyleID="TS6Cell6" />
          <Cell ss:StyleID="TS6Cell6"><Data ss:Type="Number"><xsl:value-of select="data_v" /></Data></Cell>
          <Cell ss:StyleID="TS6Cell6"><Data ss:Type="Number"><xsl:value-of select="data_kr" /></Data></Cell>
        </Row>
      </xsl:when>
    </xsl:choose>
  </xsl:template>
  <!-- <<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<< Шаблон данных листа ТХ7 >>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>> -->
  <xsl:template match="TS7">
    <xsl:choose>
      <xsl:when test="style_id = 1">
        <Row>
          <Cell ss:StyleID="TS7Cell1"><Data ss:Type="String"><xsl:value-of select="reg" /></Data></Cell>
          <Cell ss:StyleID="TS7Cell1"><Data ss:Type="String"><xsl:value-of select="city" /></Data></Cell>
          <Cell ss:StyleID="TS7Cell1"><Data ss:Type="String"><xsl:value-of select="boiler" /></Data></Cell>
          <Cell ss:StyleID="TS7Cell1"><Data ss:Type="String"><xsl:value-of select="type_o" /></Data></Cell>
          <Cell ss:StyleID="TS7Cell1"><Data ss:Type="String"><xsl:value-of select="v" /></Data></Cell>
          <Cell ss:StyleID="TS7Cell1"><Data ss:Type="String"><xsl:value-of select="m" /></Data></Cell>
          <Cell ss:StyleID="TS7Cell1"><Data ss:Type="Number"><xsl:value-of select="dv" /></Data></Cell>
          <Cell ss:StyleID="TS7Cell1"><Data ss:Type="Number"><xsl:value-of select="ax" /></Data></Cell>
          <Cell ss:StyleID="TS7Cell1"><Data ss:Type="Number"><xsl:value-of select="q" /></Data></Cell>
          <Cell ss:StyleID="TS7Cell1"><Data ss:Type="Number"><xsl:value-of select="aq" /></Data></Cell>
          <Cell ss:StyleID="TS7Cell1"><Data ss:Type="Number"><xsl:value-of select="pd" /></Data></Cell>
          <Cell ss:StyleID="TS7Cell1"><Data ss:Type="Number"><xsl:value-of select="ph" /></Data></Cell>
          <Cell ss:StyleID="TS7Cell1"><Data ss:Type="Number"><xsl:value-of select="k" /></Data></Cell>
          <Cell ss:StyleID="TS7Cell1"><Data ss:Type="Number"><xsl:value-of select="Se" /></Data></Cell>
          <Cell ss:StyleID="TS7Cell1"><Data ss:Type="Number"><xsl:value-of select="Fact_ras" /></Data></Cell>
        </Row>
      </xsl:when>
    </xsl:choose>
  </xsl:template>
  <!-- <<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<< Шаблон данных листа ТХ8 >>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>> -->
  <xsl:template match="TS81">
    <xsl:choose>
      <xsl:when test="style_id = 1">
        <!-- <<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<< Строка с улусом >>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>> -->
        <Row>
          <Cell ss:MergeAcross="10" ss:StyleID="TS8Cell1"><Data ss:Type="String"><xsl:value-of select="A" /></Data></Cell>
        </Row>
      </xsl:when>
      <xsl:when test="style_id = 2">
        <!-- <<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<< Строка с наслегом >>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>> -->
        <Row>
          <Cell ss:MergeAcross="6" ss:StyleID="TS8Cell2"><Data ss:Type="String"><xsl:value-of select="A" /></Data></Cell>
          <Cell ss:StyleID="TS8Cell2"><Data ss:Type="Number"><xsl:value-of select="H" /></Data></Cell>
          <Cell ss:MergeAcross="2" ss:StyleID="TS8Cell3" />
        </Row>
      </xsl:when>
      <xsl:when test="style_id = 3">
        <!-- <<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<< Строка с населенным пунктом >>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>> -->
        <Row>
          <Cell ss:MergeAcross="6" ss:StyleID="TS8Cell2"><Data ss:Type="String"><xsl:value-of select="A" /></Data></Cell>
          <Cell ss:StyleID="TS8Cell2"><Data ss:Type="Number"><xsl:value-of select="H" /></Data></Cell>
          <Cell ss:MergeAcross="2" ss:StyleID="TS8Cell3" />
        </Row>
      </xsl:when>
      <xsl:when test="style_id = 4">
        <!-- <<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<< Строка с котельной >>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>> -->
        <Row>
          <Cell ss:StyleID="TS8Cell3" />
          <Cell ss:MergeAcross="1" ss:StyleID="TS8Cell4"><Data ss:Type="String"><xsl:value-of select="B" /></Data></Cell>
          <Cell ss:StyleID="TS8Cell5"><Data ss:Type="String"><xsl:value-of select="D" /></Data></Cell>
          <Cell ss:StyleID="TS8Cell5"><Data ss:Type="String"><xsl:value-of select="E" /></Data></Cell>
          <Cell ss:StyleID="TS8Cell5"><Data ss:Type="Number"><xsl:value-of select="F" /></Data></Cell>
          <Cell ss:StyleID="TS8Cell3" />
          <Cell ss:StyleID="TS8Cell2"><Data ss:Type="Number"><xsl:value-of select="H" /></Data></Cell>
          <Cell ss:StyleID="TS8Cell5"><Data ss:Type="Number"><xsl:value-of select="I" /></Data></Cell>
          <Cell ss:MergeAcross="1" ss:StyleID="TS8Cell3" />
        </Row>
      </xsl:when>
      <xsl:when test="style_id = 5">
        <!-- <<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<< Строка с трубами >>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>> -->
        <Row>
          <Cell ss:MergeAcross="1" ss:StyleID="TS8Cell6" />
          <Cell ss:StyleID="TS8Cell7"><Data ss:Type="Number"><xsl:value-of select="C" /></Data></Cell>
          <Cell ss:MergeAcross="2" ss:StyleID="TS8Cell6" />
          <Cell ss:StyleID="TS8Cell7"><Data ss:Type="Number"><xsl:value-of select="G" /></Data></Cell>
          <Cell ss:StyleID="TS8Cell6"><Data ss:Type="Number"><xsl:value-of select="H" /></Data></Cell>
          <Cell ss:StyleID="TS8Cell7"><Data ss:Type="Number"><xsl:value-of select="I" /></Data></Cell>
          <Cell ss:StyleID="TS8Cell6"><Data ss:Type="String"><xsl:value-of select="J" /></Data></Cell>
          <Cell ss:StyleID="TS8Cell6"><Data ss:Type="String"><xsl:value-of select="K" /></Data></Cell>
        </Row>
      </xsl:when>
    </xsl:choose>
  </xsl:template>
  <!-- <<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<< Шаблон данных листа ТХ8 >>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>> -->
  <xsl:template match="TS82">
    <xsl:choose>
      <xsl:when test="style_id = 1">
        <!-- <<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<< Строка с улусом >>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>> -->
        <Row>
          <Cell ss:MergeAcross="10" ss:StyleID="TS8Cell1"><Data ss:Type="String"><xsl:value-of select="A" /></Data></Cell>
        </Row>
      </xsl:when>
      <xsl:when test="style_id = 2">
        <!-- <<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<< Строка с наслегом >>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>> -->
        <Row>
          <Cell ss:MergeAcross="6" ss:StyleID="TS8Cell2"><Data ss:Type="String"><xsl:value-of select="A" /></Data></Cell>
          <Cell ss:StyleID="TS8Cell2"><Data ss:Type="Number"><xsl:value-of select="H" /></Data></Cell>
          <Cell ss:MergeAcross="2" ss:StyleID="TS8Cell3" />
        </Row>
      </xsl:when>
      <xsl:when test="style_id = 3">
        <!-- <<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<< Строка с населенным пунктом >>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>> -->
        <Row>
          <Cell ss:MergeAcross="6" ss:StyleID="TS8Cell2"><Data ss:Type="String"><xsl:value-of select="A" /></Data></Cell>
          <Cell ss:StyleID="TS8Cell2"><Data ss:Type="Number"><xsl:value-of select="H" /></Data></Cell>
          <Cell ss:MergeAcross="2" ss:StyleID="TS8Cell3" />
        </Row>
      </xsl:when>
      <xsl:when test="style_id = 4">
        <!-- <<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<< Строка с котельной >>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>> -->
        <Row>
          <Cell ss:StyleID="TS8Cell3" />
          <Cell ss:MergeAcross="1" ss:StyleID="TS8Cell4"><Data ss:Type="String"><xsl:value-of select="B" /></Data></Cell>
          <Cell ss:StyleID="TS8Cell5"><Data ss:Type="String"><xsl:value-of select="D" /></Data></Cell>
          <Cell ss:StyleID="TS8Cell5"><Data ss:Type="String"><xsl:value-of select="E" /></Data></Cell>
          <Cell ss:StyleID="TS8Cell5"><Data ss:Type="Number"><xsl:value-of select="F" /></Data></Cell>
          <Cell ss:StyleID="TS8Cell3" />
          <Cell ss:StyleID="TS8Cell2"><Data ss:Type="Number"><xsl:value-of select="H" /></Data></Cell>
          <Cell ss:StyleID="TS8Cell5"><Data ss:Type="Number"><xsl:value-of select="I" /></Data></Cell>
          <Cell ss:MergeAcross="1" ss:StyleID="TS8Cell3" />
        </Row>
      </xsl:when>
      <xsl:when test="style_id = 5">
        <!-- <<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<< Строка с трубами >>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>> -->
        <Row>
          <Cell ss:MergeAcross="1" ss:StyleID="TS8Cell6" />
          <Cell ss:StyleID="TS8Cell7"><Data ss:Type="Number"><xsl:value-of select="C" /></Data></Cell>
          <Cell ss:MergeAcross="2" ss:StyleID="TS8Cell6" />
          <Cell ss:StyleID="TS8Cell7"><Data ss:Type="Number"><xsl:value-of select="G" /></Data></Cell>
          <Cell ss:StyleID="TS8Cell6"><Data ss:Type="Number"><xsl:value-of select="H" /></Data></Cell>
          <Cell ss:StyleID="TS8Cell7"><Data ss:Type="Number"><xsl:value-of select="I" /></Data></Cell>
          <Cell ss:StyleID="TS8Cell6"><Data ss:Type="String"><xsl:value-of select="J" /></Data></Cell>
          <Cell ss:StyleID="TS8Cell6"><Data ss:Type="String"><xsl:value-of select="K" /></Data></Cell>
        </Row>
      </xsl:when>
    </xsl:choose>
  </xsl:template>
  <!-- <<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<< Шаблон данных листа ТХ8 >>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>> -->
  <xsl:template match="TS83">
    <xsl:choose>
      <xsl:when test="style_id = 1">
        <!-- <<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<< Строка с улусом >>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>> -->
        <Row>
          <Cell ss:MergeAcross="10" ss:StyleID="TS8Cell1"><Data ss:Type="String"><xsl:value-of select="A" /></Data></Cell>
        </Row>
      </xsl:when>
      <xsl:when test="style_id = 2">
        <!-- <<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<< Строка с наслегом >>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>> -->
        <Row>
          <Cell ss:MergeAcross="6" ss:StyleID="TS8Cell2"><Data ss:Type="String"><xsl:value-of select="A" /></Data></Cell>
          <Cell ss:StyleID="TS8Cell2"><Data ss:Type="Number"><xsl:value-of select="H" /></Data></Cell>
          <Cell ss:MergeAcross="2" ss:StyleID="TS8Cell3" />
        </Row>
      </xsl:when>
      <xsl:when test="style_id = 3">
        <!-- <<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<< Строка с населенным пунктом >>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>> -->
        <Row>
          <Cell ss:MergeAcross="6" ss:StyleID="TS8Cell2"><Data ss:Type="String"><xsl:value-of select="A" /></Data></Cell>
          <Cell ss:StyleID="TS8Cell2"><Data ss:Type="Number"><xsl:value-of select="H" /></Data></Cell>
          <Cell ss:MergeAcross="2" ss:StyleID="TS8Cell3" />
        </Row>
      </xsl:when>
      <xsl:when test="style_id = 4">
        <!-- <<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<< Строка с котельной >>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>> -->
        <Row>
          <Cell ss:StyleID="TS8Cell3" />
          <Cell ss:MergeAcross="1" ss:StyleID="TS8Cell4"><Data ss:Type="String"><xsl:value-of select="B" /></Data></Cell>
          <Cell ss:StyleID="TS8Cell5"><Data ss:Type="String"><xsl:value-of select="D" /></Data></Cell>
          <Cell ss:StyleID="TS8Cell5"><Data ss:Type="String"><xsl:value-of select="E" /></Data></Cell>
          <Cell ss:StyleID="TS8Cell5"><Data ss:Type="Number"><xsl:value-of select="F" /></Data></Cell>
          <Cell ss:StyleID="TS8Cell3" />
          <Cell ss:StyleID="TS8Cell2"><Data ss:Type="Number"><xsl:value-of select="H" /></Data></Cell>
          <Cell ss:StyleID="TS8Cell5"><Data ss:Type="Number"><xsl:value-of select="I" /></Data></Cell>
          <Cell ss:MergeAcross="1" ss:StyleID="TS8Cell3" />
        </Row>
      </xsl:when>
      <xsl:when test="style_id = 5">
        <!-- <<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<< Строка с трубами >>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>> -->
        <Row>
          <Cell ss:MergeAcross="1" ss:StyleID="TS8Cell6" />
          <Cell ss:StyleID="TS8Cell7"><Data ss:Type="Number"><xsl:value-of select="C" /></Data></Cell>
          <Cell ss:MergeAcross="2" ss:StyleID="TS8Cell6" />
          <Cell ss:StyleID="TS8Cell7"><Data ss:Type="Number"><xsl:value-of select="G" /></Data></Cell>
          <Cell ss:StyleID="TS8Cell6"><Data ss:Type="Number"><xsl:value-of select="H" /></Data></Cell>
          <Cell ss:StyleID="TS8Cell7"><Data ss:Type="Number"><xsl:value-of select="I" /></Data></Cell>
          <Cell ss:StyleID="TS8Cell6"><Data ss:Type="String"><xsl:value-of select="J" /></Data></Cell>
          <Cell ss:StyleID="TS8Cell6"><Data ss:Type="String"><xsl:value-of select="K" /></Data></Cell>
        </Row>
      </xsl:when>
    </xsl:choose>
  </xsl:template>
  <!-- <<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<< Шаблон данных листа ТХ8 >>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>> -->
  <xsl:template match="TS84">
    <xsl:choose>
      <xsl:when test="style_id = 1">
        <!-- <<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<< Строка с улусом >>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>> -->
        <Row>
          <Cell ss:MergeAcross="10" ss:StyleID="TS8Cell1"><Data ss:Type="String"><xsl:value-of select="A" /></Data></Cell>
        </Row>
      </xsl:when>
      <xsl:when test="style_id = 2">
        <!-- <<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<< Строка с наслегом >>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>> -->
        <Row>
          <Cell ss:MergeAcross="6" ss:StyleID="TS8Cell2"><Data ss:Type="String"><xsl:value-of select="A" /></Data></Cell>
          <Cell ss:StyleID="TS8Cell2"><Data ss:Type="Number"><xsl:value-of select="H" /></Data></Cell>
          <Cell ss:MergeAcross="2" ss:StyleID="TS8Cell3" />
        </Row>
      </xsl:when>
      <xsl:when test="style_id = 3">
        <!-- <<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<< Строка с населенным пунктом >>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>> -->
        <Row>
          <Cell ss:MergeAcross="6" ss:StyleID="TS8Cell2"><Data ss:Type="String"><xsl:value-of select="A" /></Data></Cell>
          <Cell ss:StyleID="TS8Cell2"><Data ss:Type="Number"><xsl:value-of select="H" /></Data></Cell>
          <Cell ss:MergeAcross="2" ss:StyleID="TS8Cell3" />
        </Row>
      </xsl:when>
      <xsl:when test="style_id = 4">
        <!-- <<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<< Строка с котельной >>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>> -->
        <Row>
          <Cell ss:StyleID="TS8Cell3" />
          <Cell ss:MergeAcross="1" ss:StyleID="TS8Cell4"><Data ss:Type="String"><xsl:value-of select="B" /></Data></Cell>
          <Cell ss:StyleID="TS8Cell5"><Data ss:Type="String"><xsl:value-of select="D" /></Data></Cell>
          <Cell ss:StyleID="TS8Cell5"><Data ss:Type="String"><xsl:value-of select="E" /></Data></Cell>
          <Cell ss:StyleID="TS8Cell5"><Data ss:Type="Number"><xsl:value-of select="F" /></Data></Cell>
          <Cell ss:StyleID="TS8Cell3" />
          <Cell ss:StyleID="TS8Cell2"><Data ss:Type="Number"><xsl:value-of select="H" /></Data></Cell>
          <Cell ss:StyleID="TS8Cell5"><Data ss:Type="Number"><xsl:value-of select="I" /></Data></Cell>
          <Cell ss:MergeAcross="1" ss:StyleID="TS8Cell3" />
        </Row>
      </xsl:when>
      <xsl:when test="style_id = 5">
        <!-- <<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<< Строка с трубами >>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>> -->
        <Row>
          <Cell ss:MergeAcross="1" ss:StyleID="TS8Cell6" />
          <Cell ss:StyleID="TS8Cell7"><Data ss:Type="Number"><xsl:value-of select="C" /></Data></Cell>
          <Cell ss:MergeAcross="2" ss:StyleID="TS8Cell6" />
          <Cell ss:StyleID="TS8Cell7"><Data ss:Type="Number"><xsl:value-of select="G" /></Data></Cell>
          <Cell ss:StyleID="TS8Cell6"><Data ss:Type="Number"><xsl:value-of select="H" /></Data></Cell>
          <Cell ss:StyleID="TS8Cell7"><Data ss:Type="Number"><xsl:value-of select="I" /></Data></Cell>
          <Cell ss:StyleID="TS8Cell6"><Data ss:Type="String"><xsl:value-of select="J" /></Data></Cell>
          <Cell ss:StyleID="TS8Cell6"><Data ss:Type="String"><xsl:value-of select="K" /></Data></Cell>
        </Row>
      </xsl:when>
    </xsl:choose>
  </xsl:template>
  <!-- <<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<< Шаблон данных листа ТХ9 >>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>> -->
  <xsl:template match="TS9">
    <xsl:choose>
      <xsl:when test="style_id = 1">
        <!-- <<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<< Всего >>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>> -->
        <Row>
          <Cell ss:StyleID="TS9Total"><Data ss:Type="String"><xsl:value-of select="cname" /></Data></Cell>
          <Cell ss:StyleID="TS9Total" />
          <Cell ss:StyleID="TS9Total"><Data ss:Type="String"><xsl:value-of select="dv" /></Data></Cell>
          <Cell ss:StyleID="TS9Total" />
          <Cell ss:StyleID="TS9Total"><Data ss:Type="Number"><xsl:value-of select="l" /></Data></Cell>
          <Cell ss:StyleID="TS9Total" />
          <Cell ss:StyleID="TS9Total"><Data ss:Type="Number"><xsl:value-of select="Qp" /></Data></Cell>
          <Cell ss:StyleID="TS9Total"><Data ss:Type="Number"><xsl:value-of select="Qo" /></Data></Cell>
          <Cell ss:StyleID="TS9Total"><Data ss:Type="Number"><xsl:value-of select="chQp" /></Data></Cell>
          <Cell ss:StyleID="TS9Total"><Data ss:Type="Number"><xsl:value-of select="chQo" /></Data></Cell>
          <Cell ss:StyleID="TS9Total" />
          <Cell ss:StyleID="TS9Total"><Data ss:Type="Number"><xsl:value-of select="gQp" /></Data></Cell>
          <Cell ss:StyleID="TS9Total"><Data ss:Type="Number"><xsl:value-of select="gQo" /></Data></Cell>
          <Cell ss:StyleID="TS9Total" />
          <Cell ss:StyleID="TS9Total"><Data ss:Type="Number"><xsl:value-of select="Vwin" /></Data></Cell>
          <Cell ss:StyleID="TS9Total"><Data ss:Type="Number"><xsl:value-of select="Vsum" /></Data></Cell>
          <Cell ss:StyleID="TS9Total"><Data ss:Type="Number"><xsl:value-of select="Vavg" /></Data></Cell>
          <Cell ss:StyleID="TS9Total"><Data ss:Type="Number"><xsl:value-of select="Vut" /></Data></Cell>
          <Cell ss:StyleID="TS9Total"><Data ss:Type="Number"><xsl:value-of select="Vzap" /></Data></Cell>
          <Cell ss:StyleID="TS9Total"><Data ss:Type="Number"><xsl:value-of select="Vrr" /></Data></Cell>
          <Cell ss:StyleID="TS9Total"><Data ss:Type="Number"><xsl:value-of select="Vobch" /></Data></Cell>
          <Cell ss:StyleID="TS9Total"><Data ss:Type="Number"><xsl:value-of select="yVut" /></Data></Cell>
          <Cell ss:StyleID="TS9Total"><Data ss:Type="Number"><xsl:value-of select="yTz" /></Data></Cell>
          <Cell ss:StyleID="TS9Total"><Data ss:Type="Number"><xsl:value-of select="Qob" /></Data></Cell>
        </Row>
      </xsl:when>
      <xsl:when test="style_id = 2">
        <!-- <<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<< Строка с котельной (всего) >>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>> -->
        <Row>
          <Cell ss:StyleID="TS9Cell1" />
          <Cell ss:StyleID="TS9Cell1"><Data ss:Type="String"><xsl:value-of select="bname" /></Data></Cell>
          <Cell ss:StyleID="TS9Cell1"><Data ss:Type="String"><xsl:value-of select="dv" /></Data></Cell>
          <Cell ss:StyleID="TS9Cell1" />
          <Cell ss:StyleID="TS9Cell1"><Data ss:Type="Number"><xsl:value-of select="l" /></Data></Cell>
          <Cell ss:StyleID="TS9Cell1" />
          <Cell ss:StyleID="TS9Cell1"><Data ss:Type="Number"><xsl:value-of select="Qp" /></Data></Cell>
          <Cell ss:StyleID="TS9Cell1"><Data ss:Type="Number"><xsl:value-of select="Qo" /></Data></Cell>
          <Cell ss:StyleID="TS9Cell1"><Data ss:Type="Number"><xsl:value-of select="chQp" /></Data></Cell>
          <Cell ss:StyleID="TS9Cell1"><Data ss:Type="Number"><xsl:value-of select="chQo" /></Data></Cell>
          <Cell ss:StyleID="TS9Cell1" />
          <Cell ss:StyleID="TS9Cell1"><Data ss:Type="Number"><xsl:value-of select="gQp" /></Data></Cell>
          <Cell ss:StyleID="TS9Cell1"><Data ss:Type="Number"><xsl:value-of select="gQo" /></Data></Cell>
          <Cell ss:StyleID="TS9Cell1" />
          <Cell ss:StyleID="TS9Cell1"><Data ss:Type="Number"><xsl:value-of select="Vwin" /></Data></Cell>
          <Cell ss:StyleID="TS9Cell1"><Data ss:Type="Number"><xsl:value-of select="Vsum" /></Data></Cell>
          <Cell ss:StyleID="TS9Cell1"><Data ss:Type="Number"><xsl:value-of select="Vavg" /></Data></Cell>
          <Cell ss:StyleID="TS9Cell1"><Data ss:Type="Number"><xsl:value-of select="Vut" /></Data></Cell>
          <Cell ss:StyleID="TS9Cell1"><Data ss:Type="Number"><xsl:value-of select="Vzap" /></Data></Cell>
          <Cell ss:StyleID="TS9Cell1"><Data ss:Type="Number"><xsl:value-of select="Vrr" /></Data></Cell>
          <Cell ss:StyleID="TS9Cell1"><Data ss:Type="Number"><xsl:value-of select="Vobch" /></Data></Cell>
          <Cell ss:StyleID="TS9Cell1"><Data ss:Type="Number"><xsl:value-of select="yVut" /></Data></Cell>
          <Cell ss:StyleID="TS9Cell1"><Data ss:Type="Number"><xsl:value-of select="yTz" /></Data></Cell>
          <Cell ss:StyleID="TS9Cell1"><Data ss:Type="Number"><xsl:value-of select="Qob" /></Data></Cell>
        </Row>
      </xsl:when>
      <xsl:when test="style_id = 3">
        <!-- <<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<< Строка с котельной >>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>> -->
        <Row>
          <Cell ss:StyleID="TS9Cell1" />
          <Cell ss:StyleID="TS9Cell1"><Data ss:Type="String"><xsl:value-of select="bname" /></Data></Cell>
          <Cell ss:StyleID="TS9Cell1"><Data ss:Type="Number"><xsl:value-of select="dv" /></Data></Cell>
          <Cell ss:StyleID="TS9Cell2"><Data ss:Type="Number"><xsl:value-of select="ud" /></Data></Cell>
          <Cell ss:StyleID="TS9Cell2"><Data ss:Type="Number"><xsl:value-of select="l" /></Data></Cell>
          <Cell ss:StyleID="TS9Cell2"><Data ss:Type="Number"><xsl:value-of select="kp" /></Data></Cell>
          <Cell ss:StyleID="TS9Cell2"><Data ss:Type="Number"><xsl:value-of select="Qp" /></Data></Cell>
          <Cell ss:StyleID="TS9Cell2"><Data ss:Type="Number"><xsl:value-of select="Qo" /></Data></Cell>
          <Cell ss:StyleID="TS9Cell2"><Data ss:Type="Number"><xsl:value-of select="chQp" /></Data></Cell>
          <Cell ss:StyleID="TS9Cell2"><Data ss:Type="Number"><xsl:value-of select="chQo" /></Data></Cell>
          <Cell ss:StyleID="TS9Cell2"><Data ss:Type="Number"><xsl:value-of select="god" /></Data></Cell>
          <Cell ss:StyleID="TS9Cell2"><Data ss:Type="Number"><xsl:value-of select="gQp" /></Data></Cell>
          <Cell ss:StyleID="TS9Cell2"><Data ss:Type="Number"><xsl:value-of select="gQo" /></Data></Cell>
          <Cell ss:StyleID="TS9Cell2"><Data ss:Type="Number"><xsl:value-of select="Sf" /></Data></Cell>
          <Cell ss:StyleID="TS9Cell2"><Data ss:Type="Number"><xsl:value-of select="Vwin" /></Data></Cell>
          <Cell ss:StyleID="TS9Cell2"><Data ss:Type="Number"><xsl:value-of select="Vsum" /></Data></Cell>
          <Cell ss:StyleID="TS9Cell2"><Data ss:Type="Number"><xsl:value-of select="Vavg" /></Data></Cell>
          <Cell ss:StyleID="TS9Cell2"><Data ss:Type="Number"><xsl:value-of select="Vut" /></Data></Cell>
          <Cell ss:StyleID="TS9Cell2"><Data ss:Type="Number"><xsl:value-of select="Vzap" /></Data></Cell>
          <Cell ss:StyleID="TS9Cell2"><Data ss:Type="Number"><xsl:value-of select="Vrr" /></Data></Cell>
          <Cell ss:StyleID="TS9Cell2"><Data ss:Type="Number"><xsl:value-of select="Vobch" /></Data></Cell>
          <Cell ss:StyleID="TS9Cell2"><Data ss:Type="Number"><xsl:value-of select="yVut" /></Data></Cell>
          <Cell ss:StyleID="TS9Cell2"><Data ss:Type="Number"><xsl:value-of select="yTz" /></Data></Cell>
          <Cell ss:StyleID="TS9Cell2"><Data ss:Type="Number"><xsl:value-of select="Qob" /></Data></Cell>
        </Row>
      </xsl:when>
    </xsl:choose>
  </xsl:template>
</xsl:stylesheet>