<?xml version="1.0" encoding="utf-8" ?>
<!--
// Copyright © 2004-2014 Tim Van Holder, Windower Team
// 
// Licensed under the Apache License, Version 2.0 (the "License"); you may not use this file except in compliance with the License.
// You may obtain a copy of the License at http://www.apache.org/licenses/LICENSE-2.0
// 
// Unless required by applicable law or agreed to in writing, software distributed under the License is distributed on an "AS IS"
// BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and limitations under the License.
-->
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" version="1.0">

  <xsl:output method="xml" version="1.0" encoding="utf-8" omit-xml-declaration="no" media-type="text/xml" indent="yes"/>

  <!-- Step 1: Try to figure out which format it is. -->
  <xsl:template match="/">
    <xsl:choose>
      <xsl:when test="/ffxi-item-info">
	<xsl:apply-templates mode="format-1" select="/ffxi-item-info"/>
      </xsl:when>
      <xsl:when test="/ItemList">
	<xsl:apply-templates mode="format-2" select="/ItemList"/>
      </xsl:when>
      <xsl:when test="/thing-list">
	<xsl:apply-templates mode="format-3" select="/thing-list"/>
      </xsl:when>
      <xsl:otherwise>
	<xsl:message terminate="yes">Sorry, this file could not be recognized as an item dump.</xsl:message>
      </xsl:otherwise>
    </xsl:choose>
  </xsl:template>

  <xsl:template name="error-exit">
    <xsl:message terminate="yes">An error occurred trying to process this file.</xsl:message>
  </xsl:template>
  
  <!--==========-->
  <!-- Format 1 -->
  <!--==========-->

  <xsl:template mode="format-1" match="/ffxi-item-info">
    <thing-list>
      <xsl:for-each select="item">
	<thing type="Item">
	  <xsl:apply-templates mode="format-1" select="."/>
	</thing>
      </xsl:for-each>
    </thing-list>
  </xsl:template>

  <xsl:template mode="format-1" match="/ffxi-item-info/item">
    <xsl:for-each select="field">
      <xsl:choose>
	<xsl:when test="@name = 'EnglishName'">
	  <xsl:if test="/ffxi-item-info/data-language = 'English'">
	    <field>
	      <xsl:attribute name="name">name</xsl:attribute>
	      <xsl:value-of select="."/>
	    </field>
	  </xsl:if>
	</xsl:when>
	<xsl:when test="@name = 'JapaneseName'">
	  <xsl:if test="/ffxi-item-info/data-language = 'Japanese'">
	    <field>
	      <xsl:attribute name="name">name</xsl:attribute>
	      <xsl:value-of select="."/>
	    </field>
	  </xsl:if>
	</xsl:when>
	<xsl:otherwise>
	  <field>
	    <xsl:apply-templates mode="format-1" select="."/>
	  </field>
	</xsl:otherwise>
      </xsl:choose>
    </xsl:for-each>
    <!-- Calculate DPS -->
    <xsl:if test="./field[@name = 'Damage'] and ./field[@name = 'Delay']">
      <field name="dps"><xsl:value-of select="round(6000 * number(./field[@name = 'Damage']) div number(./field[@name = 'Delay']))"/></field>
    </xsl:if>
    <xsl:apply-templates mode="format-1" select="icon"/>
  </xsl:template>

  <xsl:template mode="format-1" match="/ffxi-item-info/item/field">
    <!-- Default field rule: map the field name and preserve the value -->
    <xsl:apply-templates mode="format-1-map-field-name" select="."/>
    <xsl:value-of select="."/>
  </xsl:template>

  <xsl:template mode="format-1-map-field-name" match="/ffxi-item-info/item/field">
    <xsl:attribute name="name">
      <xsl:call-template name="map-field-name">
	<xsl:with-param name="field-name" select="@name"/>
      </xsl:call-template>
    </xsl:attribute>
  </xsl:template>

  <xsl:template mode="format-1" match="/ffxi-item-info/item/field[@name = 'Flags']">
    <xsl:apply-templates mode="format-1-map-field-name" select="."/>
    <xsl:variable name="flag01">0<xsl:if test="contains(., 'Flag00')"><xsl:value-of select="1"/></xsl:if></xsl:variable>
    <xsl:variable name="flag02">0<xsl:if test="contains(., 'Flag01')"><xsl:value-of select="2"/></xsl:if></xsl:variable>
    <xsl:variable name="flag03">0<xsl:if test="contains(., 'Flag02')"><xsl:value-of select="4"/></xsl:if></xsl:variable>
    <xsl:variable name="flag04">0<xsl:if test="contains(., 'Flag03')"><xsl:value-of select="8"/></xsl:if></xsl:variable>
    <xsl:variable name="flag05">0<xsl:if test="contains(., 'Flag04')"><xsl:value-of select="16"/></xsl:if></xsl:variable>
    <xsl:variable name="flag06">0<xsl:if test="contains(., 'Flag05') or contains(., 'Inscribable')"><xsl:value-of select="32"/></xsl:if></xsl:variable>
    <xsl:variable name="flag07">0<xsl:if test="contains(., 'Flag06') or contains(., 'NoAuction') or contains(., 'Ex')"><xsl:value-of select="64"/></xsl:if></xsl:variable>
    <xsl:variable name="flag08">0<xsl:if test="contains(., 'Flag07') or contains(., 'Scroll')"><xsl:value-of select="128"/></xsl:if></xsl:variable>
    <xsl:variable name="flag09">0<xsl:if test="contains(., 'Flag08') or contains(., 'Linkshell')"><xsl:value-of select="256"/></xsl:if></xsl:variable>
    <xsl:variable name="flag10">0<xsl:if test="contains(., 'Flag09') or contains(., 'CanUse')"><xsl:value-of select="512"/></xsl:if></xsl:variable>
    <xsl:variable name="flag11">0<xsl:if test="contains(., 'Flag10') or contains(., 'CanTradeNPC')"><xsl:value-of select="1024"/></xsl:if></xsl:variable>
    <xsl:variable name="flag12">0<xsl:if test="contains(., 'Flag11') or contains(., 'CanEquip')"><xsl:value-of select="2048"/></xsl:if></xsl:variable>
    <xsl:variable name="flag13">0<xsl:if test="contains(., 'Flag12') or contains(., 'NoSale')"><xsl:value-of select="4096"/></xsl:if></xsl:variable>
    <xsl:variable name="flag14">0<xsl:if test="contains(., 'Flag13') or contains(., 'NoDelivery') or contains(., 'Ex')"><xsl:value-of select="08192"/></xsl:if></xsl:variable>
    <xsl:variable name="flag15">0<xsl:if test="contains(., 'Flag14') or contains(., 'NoTradePC') or contains(., 'Ex')"><xsl:value-of select="16384"/></xsl:if></xsl:variable>
    <xsl:variable name="flag16">0<xsl:if test="contains(., 'Flag15') or contains(., 'Rare')"><xsl:value-of select="32768"/></xsl:if></xsl:variable>
    <xsl:call-template name="format-number-as-hex">
      <xsl:with-param name="number" select="$flag01 + $flag02 + $flag03 + $flag04 + $flag05 + $flag06 + $flag07 + $flag08 + $flag09 + $flag10 + $flag11 + $flag12 + $flag13 + $flag14 + $flag15 + $flag16"/>
    </xsl:call-template>
  </xsl:template>

  <xsl:template mode="format-1" match="/ffxi-item-info/item/field[@name = 'Jobs']">
    <xsl:apply-templates mode="format-1-map-field-name" select="."/>
    <xsl:variable name="flag01">0</xsl:variable><!-- Unused -->
    <xsl:variable name="flag02">0<xsl:if test=". = 'All' or contains(., 'WAR')"><xsl:value-of select="2"/></xsl:if></xsl:variable>
    <xsl:variable name="flag03">0<xsl:if test=". = 'All' or contains(., 'MNK')"><xsl:value-of select="4"/></xsl:if></xsl:variable>
    <xsl:variable name="flag04">0<xsl:if test=". = 'All' or contains(., 'WHM')"><xsl:value-of select="8"/></xsl:if></xsl:variable>
    <xsl:variable name="flag05">0<xsl:if test=". = 'All' or contains(., 'BLM')"><xsl:value-of select="16"/></xsl:if></xsl:variable>
    <xsl:variable name="flag06">0<xsl:if test=". = 'All' or contains(., 'RDM')"><xsl:value-of select="32"/></xsl:if></xsl:variable>
    <xsl:variable name="flag07">0<xsl:if test=". = 'All' or contains(., 'THF')"><xsl:value-of select="64"/></xsl:if></xsl:variable>
    <xsl:variable name="flag08">0<xsl:if test=". = 'All' or contains(., 'PLD')"><xsl:value-of select="128"/></xsl:if></xsl:variable>
    <xsl:variable name="flag09">0<xsl:if test=". = 'All' or contains(., 'DRK')"><xsl:value-of select="256"/></xsl:if></xsl:variable>
    <xsl:variable name="flag10">0<xsl:if test=". = 'All' or contains(., 'BST')"><xsl:value-of select="512"/></xsl:if></xsl:variable>
    <xsl:variable name="flag11">0<xsl:if test=". = 'All' or contains(., 'BRD')"><xsl:value-of select="1024"/></xsl:if></xsl:variable>
    <xsl:variable name="flag12">0<xsl:if test=". = 'All' or contains(., 'RNG')"><xsl:value-of select="2048"/></xsl:if></xsl:variable>
    <xsl:variable name="flag13">0<xsl:if test=". = 'All' or contains(., 'SAM')"><xsl:value-of select="4096"/></xsl:if></xsl:variable>
    <xsl:variable name="flag14">0<xsl:if test=". = 'All' or contains(., 'NIN')"><xsl:value-of select="08192"/></xsl:if></xsl:variable>
    <xsl:variable name="flag15">0<xsl:if test=". = 'All' or contains(., 'DRG')"><xsl:value-of select="16384"/></xsl:if></xsl:variable>
    <xsl:variable name="flag16">0<xsl:if test=". = 'All' or contains(., 'SMN')"><xsl:value-of select="32768"/></xsl:if></xsl:variable>
    <xsl:call-template name="format-number-as-hex">
      <xsl:with-param name="number" select="$flag01 + $flag02 + $flag03 + $flag04 + $flag05 + $flag06 + $flag07 + $flag08 + $flag09 + $flag10 + $flag11 + $flag12 + $flag13 + $flag14 + $flag15 + $flag16"/>
    </xsl:call-template>
  </xsl:template>

  <xsl:template mode="format-1" match="/ffxi-item-info/item/field[@name = 'Slots']">
    <xsl:apply-templates mode="format-1-map-field-name" select="."/>
    <xsl:variable name="flag01">0<xsl:if test=". = 'All' or contains(., 'Main')"><xsl:value-of select="1"/></xsl:if></xsl:variable>
    <xsl:variable name="flag02">0<xsl:if test=". = 'All' or contains(., 'Sub')"><xsl:value-of select="2"/></xsl:if></xsl:variable>
    <xsl:variable name="flag03">0<xsl:if test=". = 'All' or contains(., 'Range')"><xsl:value-of select="4"/></xsl:if></xsl:variable>
    <xsl:variable name="flag04">0<xsl:if test=". = 'All' or contains(., 'Ammo')"><xsl:value-of select="8"/></xsl:if></xsl:variable>
    <xsl:variable name="flag05">0<xsl:if test=". = 'All' or contains(., 'Head')"><xsl:value-of select="16"/></xsl:if></xsl:variable>
    <xsl:variable name="flag06">0<xsl:if test=". = 'All' or contains(., 'Body')"><xsl:value-of select="32"/></xsl:if></xsl:variable>
    <xsl:variable name="flag07">0<xsl:if test=". = 'All' or contains(., 'Hands')"><xsl:value-of select="64"/></xsl:if></xsl:variable>
    <xsl:variable name="flag08">0<xsl:if test=". = 'All' or contains(., 'Legs')"><xsl:value-of select="128"/></xsl:if></xsl:variable>
    <xsl:variable name="flag09">0<xsl:if test=". = 'All' or contains(., 'Feet')"><xsl:value-of select="256"/></xsl:if></xsl:variable>
    <xsl:variable name="flag10">0<xsl:if test=". = 'All' or contains(., 'Neck')"><xsl:value-of select="512"/></xsl:if></xsl:variable>
    <xsl:variable name="flag11">0<xsl:if test=". = 'All' or contains(., 'Waist')"><xsl:value-of select="1024"/></xsl:if></xsl:variable>
    <xsl:variable name="flag12">0<xsl:if test=". = 'All' or contains(., 'LEar') or contains(., 'Ears')"><xsl:value-of select="2048"/></xsl:if></xsl:variable>
    <xsl:variable name="flag13">0<xsl:if test=". = 'All' or contains(., 'REar') or contains(., 'Ears')"><xsl:value-of select="4096"/></xsl:if></xsl:variable>
    <xsl:variable name="flag14">0<xsl:if test=". = 'All' or contains(., 'LRing') or contains(., 'Rings')"><xsl:value-of select="08192"/></xsl:if></xsl:variable>
    <xsl:variable name="flag15">0<xsl:if test=". = 'All' or contains(., 'RRing') or contains(., 'Rings')"><xsl:value-of select="16384"/></xsl:if></xsl:variable>
    <xsl:variable name="flag16">0<xsl:if test=". = 'All' or contains(., 'Back') or contains(., 'Back')"><xsl:value-of select="32768"/></xsl:if></xsl:variable>
    <xsl:call-template name="format-number-as-hex">
      <xsl:with-param name="number" select="$flag01 + $flag02 + $flag03 + $flag04 + $flag05 + $flag06 + $flag07 + $flag08 + $flag09 + $flag10 + $flag11 + $flag12 + $flag13 + $flag14 + $flag15 + $flag16"/>
    </xsl:call-template>
  </xsl:template>

  <xsl:template mode="format-1" match="/ffxi-item-info/item/field[@name = 'Races']">
    <xsl:apply-templates mode="format-1-map-field-name" select="."/>
    <xsl:variable name="flag01">0</xsl:variable>
    <xsl:variable name="flag02">0<xsl:if test=". = 'All' or contains(., 'HumeMale') or (contains(., 'Hume') and not (contains(., 'HumeFemale'))) or (contains(., 'Male') and not (contains(., 'ElvaanMale') or contains(., 'TarutaruMale')))"><xsl:value-of select="2"/></xsl:if></xsl:variable>
    <xsl:variable name="flag03">0<xsl:if test=". = 'All' or contains(., 'HumeFemale') or (contains(., 'Hume') and not (contains(., 'HumeMale'))) or (contains(., 'Female') and not (contains(., 'ElvaanFemale') or contains(., 'TarutaruFemale')))"><xsl:value-of select="4"/></xsl:if></xsl:variable>
    <xsl:variable name="flag04">0<xsl:if test=". = 'All' or contains(., 'ElvaanMale') or (contains(., 'Elvaan') and not (contains(., 'ElvaanFemale'))) or (contains(., 'Male') and not (contains(., 'HumeMale') or contains(., 'TarutaruMale')))"><xsl:value-of select="8"/></xsl:if></xsl:variable>
    <xsl:variable name="flag05">0<xsl:if test=". = 'All' or contains(., 'ElvaanFemale') or (contains(., 'Elvaan') and not (contains(., 'ElvaanMale'))) or (contains(., 'Female') and not (contains(., 'HumeFemale') or contains(., 'TarutaruFemale')))"><xsl:value-of select="16"/></xsl:if></xsl:variable>
    <xsl:variable name="flag06">0<xsl:if test=". = 'All' or contains(., 'TarutaruMale') or (contains(., 'Tarutaru') and not (contains(., 'TarutaruFemale'))) or (contains(., 'Male') and not (contains(., 'HumeMale') or contains(., 'ElvaanMale')))"><xsl:value-of select="32"/></xsl:if></xsl:variable>
    <xsl:variable name="flag07">0<xsl:if test=". = 'All' or contains(., 'TarutaruFemale') or (contains(., 'Tarutaru') and not (contains(., 'TarutaruMale'))) or (contains(., 'Female') and not (contains(., 'HumeFemale') or contains(., 'ElvaanFemale')))"><xsl:value-of select="64"/></xsl:if></xsl:variable>
    <xsl:variable name="flag08">0<xsl:if test=". = 'All' or contains(., 'Mithra') or (contains(., 'Female') and not (contains(., 'HumeFemale') or contains(., 'ElvaanFemale') or contains(., 'TarutaruFemale')))"><xsl:value-of select="128"/></xsl:if></xsl:variable>
    <xsl:variable name="flag09">0<xsl:if test=". = 'All' or contains(., 'Galka') or (contains(., 'Male') and not (contains(., 'HumeMale') or contains(., 'ElvaanMale') or contains(., 'TarutaruMale')))"><xsl:value-of select="256"/></xsl:if></xsl:variable>
    <xsl:variable name="flag10">0</xsl:variable>
    <xsl:variable name="flag11">0</xsl:variable>
    <xsl:variable name="flag12">0</xsl:variable>
    <xsl:variable name="flag13">0</xsl:variable>
    <xsl:variable name="flag14">0</xsl:variable>
    <xsl:variable name="flag15">0</xsl:variable>
    <xsl:variable name="flag16">0</xsl:variable>
    <xsl:call-template name="format-number-as-hex">
      <xsl:with-param name="number" select="$flag01 + $flag02 + $flag03 + $flag04 + $flag05 + $flag06 + $flag07 + $flag08 + $flag09 + $flag10 + $flag11 + $flag12 + $flag13 + $flag14 + $flag15 + $flag16"/>
    </xsl:call-template>
  </xsl:template>

  <xsl:template mode="format-1" match="/ffxi-item-info/item/field[@name = 'Skill']">
    <xsl:apply-templates mode="format-1-map-field-name" select="."/>
    <xsl:choose>
      <xsl:when test=". = 'None'">00</xsl:when>
      <xsl:when test=". = 'HandToHand'">01</xsl:when>
      <xsl:when test=". = 'Dagger'">02</xsl:when>
      <xsl:when test=". = 'Sword'">03</xsl:when>
      <xsl:when test=". = 'GreatSword'">04</xsl:when>
      <xsl:when test=". = 'Axe'">05</xsl:when>
      <xsl:when test=". = 'GreatAxe'">06</xsl:when>
      <xsl:when test=". = 'Scythe'">07</xsl:when>
      <xsl:when test=". = 'PoleArm'">08</xsl:when>
      <xsl:when test=". = 'Katana'">09</xsl:when>
      <xsl:when test=". = 'GreatKatana'">0A</xsl:when>
      <xsl:when test=". = 'Club'">0B</xsl:when>
      <xsl:when test=". = 'Staff'">0C</xsl:when>
      <xsl:when test=". = 'Ranged'">19</xsl:when>
      <xsl:when test=". = 'Marksmanship'">1A</xsl:when>
      <xsl:when test=". = 'Thrown'">1B</xsl:when>
      <xsl:when test=". = 'DivineMagic'">20</xsl:when>
      <xsl:when test=". = 'HealingMagic'">21</xsl:when>
      <xsl:when test=". = 'EnhancingMagic'">22</xsl:when>
      <xsl:when test=". = 'EnfeeblingMagic'">23</xsl:when>
      <xsl:when test=". = 'ElementalMagic'">24</xsl:when>
      <xsl:when test=". = 'DarkMagic'">25</xsl:when>
      <xsl:when test=". = 'SummoningMagic'">26</xsl:when>
      <xsl:when test=". = 'Ninjutsu'">27</xsl:when>
      <xsl:when test=". = 'Singing'">28</xsl:when>
      <xsl:when test=". = 'StringInstrument'">29</xsl:when>
      <xsl:when test=". = 'WindInstrument'">2A</xsl:when>
      <xsl:when test=". = 'Fishing'">30</xsl:when>
      <xsl:otherwise>FF</xsl:otherwise>
    </xsl:choose>
  </xsl:template>

  <xsl:template mode="format-1" match="/ffxi-item-info/item/field[@name = 'Type']">
    <xsl:apply-templates mode="format-1-map-field-name" select="."/>
    <xsl:choose>
      <xsl:when test=". = 'Nothing'">0000</xsl:when>
      <xsl:when test=". = 'Item'">0001</xsl:when>
      <xsl:when test=". = 'QuestItem'">0002</xsl:when>
      <xsl:when test=". = 'Fish'">0003</xsl:when>
      <xsl:when test=". = 'Weapon'">0004</xsl:when>
      <xsl:when test=". = 'Armor'">0005</xsl:when>
      <xsl:when test=". = 'Linkshell'">0006</xsl:when>
      <xsl:when test=". = 'UsableItem'">0007</xsl:when>
      <xsl:when test=". = 'Crystal'">0008</xsl:when>
      <xsl:when test=". = 'Unknown'">0009</xsl:when>
      <xsl:when test=". = 'Furnishing'">000A</xsl:when>
      <xsl:when test=". = 'Plant'">000B</xsl:when>
      <xsl:when test=". = 'Flowerpot'">000C</xsl:when>
      <xsl:when test=". = 'Material'">000D</xsl:when>
      <xsl:when test=". = 'Mannequin'">000E</xsl:when>
      <xsl:when test=". = 'Book'">000F</xsl:when>
      <xsl:otherwise>FFFF</xsl:otherwise>
    </xsl:choose>
  </xsl:template>

  <xsl:template mode="format-1" match="/ffxi-item-info/item/icon">
    <field name="icon">
      <thing type="Graphic">
	<xsl:if test="@category"><field name="category"><xsl:value-of select="@category"/></field></xsl:if>
	<xsl:if test="@id"><field name="id"><xsl:value-of select="@id"/></field></xsl:if>
	<xsl:if test="@format"><field name="format"><xsl:value-of select="@format"/></field></xsl:if>
	<field name="image" format="image/png" encoding="base64">
	  <xsl:value-of select="."/>
	</field>
      </thing>
    </field>
  </xsl:template>

  <xsl:template mode="format-1" match="*">
    <xsl:message>Unhandled element: <xsl:value-of select="name(.)"/></xsl:message>
    <xsl:call-template name="error-exit"/>
  </xsl:template>
  
  <!--==========-->
  <!-- Format 2 -->
  <!--==========-->

  <xsl:template mode="format-2" match="/ItemList">
    <thing-list>
      <xsl:for-each select="Item">
	<thing type="Item">
	  <xsl:apply-templates mode="format-2" select="."/>
	</thing>
      </xsl:for-each>
    </thing-list>
  </xsl:template>

  <xsl:template mode="format-2" match="/ItemList/Item">
    <xsl:for-each select="./child::*">
      <xsl:choose>
	<xsl:when test="name() = 'EnglishName'">
	  <xsl:if test="/ItemList[@Language = 'English']">
	    <field>
	      <xsl:attribute name="name">name</xsl:attribute>
	      <xsl:value-of select="."/>
	    </field>
	  </xsl:if>
	</xsl:when>
	<xsl:when test="name() = 'JapaneseName'">
	  <xsl:if test="/ItemList[@Language = 'Japanese']">
	    <field>
	      <xsl:attribute name="name">name</xsl:attribute>
	      <xsl:value-of select="."/>
	    </field>
	  </xsl:if>
	</xsl:when>
	<xsl:when test="name() = 'Icon'">
	  <field>
	    <xsl:attribute name="name">
	      <xsl:call-template name="map-field-name">
		<xsl:with-param name="field-name" select="name()"/>
	      </xsl:call-template>
	    </xsl:attribute>
	    <thing type="Graphic">
	      <xsl:if test="@Category"><field name="category"><xsl:value-of select="@Category"/></field></xsl:if>
	      <xsl:if test="@ID"><field name="id"><xsl:value-of select="@ID"/></field></xsl:if>
	      <xsl:if test="@Format"><field name="format"><xsl:value-of select="@Format"/></field></xsl:if>
	      <field name="image" format="image/png" encoding="base64">
		<xsl:value-of select="."/>
	      </field>
	    </thing>
	  </field>
	</xsl:when>
	<xsl:otherwise>
	  <field>
	    <xsl:attribute name="name">
	      <xsl:call-template name="map-field-name">
		<xsl:with-param name="field-name" select="name()"/>
	      </xsl:call-template>
	    </xsl:attribute>
	    <xsl:if test="name() = 'Jobs'">0000</xsl:if>
	    <xsl:value-of select="."/>
	  </field>
	</xsl:otherwise>
      </xsl:choose>
    </xsl:for-each>
  </xsl:template>

  <xsl:template mode="format-2" match="*">
    <xsl:message>Unhandled element: <xsl:value-of select="name(.)"/></xsl:message>
    <xsl:call-template name="error-exit"/>
  </xsl:template>
  
  <!--==========-->
  <!-- Format 3 -->
  <!--==========-->

  <xsl:template mode="format-3" match="/thing-list">
    <thing-list>
      <xsl:copy-of select="@*"/>
      <xsl:apply-templates mode="format-3" select="./thing"/>
    </thing-list>
  </xsl:template>

  <xsl:template mode="format-3" match="/thing-list/thing">
    <thing>
      <xsl:copy-of select="@*"/>
      <xsl:for-each select="field">
	<xsl:choose>
	  <!-- english-name and japanese-name were obsoleted by the 20061218 patch; either discard or rename to 'name' -->
	  <xsl:when test="@name = 'english-name'">
	    <xsl:if test="../field[@name = 'log-name-singular']">
	      <field>
		<xsl:copy-of select="@*"/>
		<xsl:attribute name="name">name</xsl:attribute>
		<xsl:copy-of select="*"/>
		<xsl:copy-of select="text()"/>
	      </field>
	    </xsl:if>
	  </xsl:when>
	  <xsl:when test="@name = 'japanese-name'">
	    <xsl:if test="not(../field[@name = 'log-name-singular'])">
	      <field>
		<xsl:copy-of select="@*"/>
		<xsl:attribute name="name">name</xsl:attribute>
		<xsl:copy-of select="*"/>
		<xsl:copy-of select="text()"/>
	      </field>
	    </xsl:if>
	  </xsl:when>
	  <xsl:otherwise>
	    <xsl:copy-of select="."/>
	  </xsl:otherwise>
	</xsl:choose>
      </xsl:for-each>
    </thing>
  </xsl:template>

  <!-- Default: field is unchanged, so copy verbatim -->
  <xsl:template mode="format-3" match="/thing-list/field">
    <xsl:copy-of select="."/>
  </xsl:template>

  <!--=============-->
  <!-- Subroutines -->
  <!--=============-->

  <xsl:template name="format-number-as-hex">
    <xsl:param name="number" select="0"/>

    <xsl:variable name="this-nybble" select="$number mod 16"/>
    <xsl:variable name="rest" select="floor($number div 16)"/>

    <xsl:if test="$rest != 0">
      <xsl:call-template name="format-number-as-hex">
	<xsl:with-param name="number" select="$rest"/>
      </xsl:call-template>
    </xsl:if>

    <xsl:choose>
      <xsl:when test="$this-nybble &gt;= 10">
	<xsl:value-of select="translate(string($this-nybble - 10), '012345', 'ABCDEF')"/>
      </xsl:when>
      <xsl:otherwise>
	<xsl:value-of select="$this-nybble"/>
      </xsl:otherwise>
    </xsl:choose>
  </xsl:template>

  <xsl:template name="map-field-name">
    <xsl:param name="field-name"/>
    <xsl:choose>
      <xsl:when test="$field-name = 'CastTime'">casting-time</xsl:when>
      <xsl:when test="$field-name = 'DPS'">dps</xsl:when>
      <xsl:when test="$field-name = 'Damage'">damage</xsl:when>
      <xsl:when test="$field-name = 'Delay'">delay</xsl:when>
      <xsl:when test="$field-name = 'Description'">description</xsl:when>
      <xsl:when test="$field-name = 'Element'">element</xsl:when>
      <xsl:when test="$field-name = 'EquipDelay'">use-delay</xsl:when>
      <xsl:when test="$field-name = 'Flags'">flags</xsl:when>
      <xsl:when test="$field-name = 'ID'">id</xsl:when>
      <xsl:when test="$field-name = 'Icon'">icon</xsl:when>
      <xsl:when test="$field-name = 'Jobs'">jobs</xsl:when>
      <xsl:when test="$field-name = 'JugSize'">jug-size</xsl:when>
      <xsl:when test="$field-name = 'Level'">level</xsl:when>
      <xsl:when test="$field-name = 'iLevel'">i-level</xsl:when>
      <xsl:when test="$field-name = 'LogNamePlural'">log-name-plural</xsl:when>
      <xsl:when test="$field-name = 'LogNameSingular'">log-name-singular</xsl:when>
      <xsl:when test="$field-name = 'MaxCharges'">max-charges</xsl:when>
      <xsl:when test="$field-name = 'MysteryField'">resource-id</xsl:when>
      <xsl:when test="$field-name = 'Races'">races</xsl:when>
      <xsl:when test="$field-name = 'ResourceID'">resource-id</xsl:when>
      <xsl:when test="$field-name = 'ReuseTimer'">reuse-delay</xsl:when>
      <xsl:when test="$field-name = 'ShieldSize'">shield-size</xsl:when>
      <xsl:when test="$field-name = 'Skill'">skill</xsl:when>
      <xsl:when test="$field-name = 'Slots'">slots</xsl:when>
      <xsl:when test="$field-name = 'StackSize'">stack-size</xsl:when>
      <xsl:when test="$field-name = 'Storage'">storage-slots</xsl:when>
      <xsl:when test="$field-name = 'Type'">type</xsl:when>
      <xsl:when test="$field-name = 'ValidTargets'">valid-targets</xsl:when>
      <!-- EnglishName and JapaneseName are handled before the field name is mapped. -->
      <xsl:otherwise><xsl:message terminate="yes">Unhandled field: '<xsl:value-of select="$field-name"/>'.</xsl:message></xsl:otherwise>
    </xsl:choose>
  </xsl:template>
  
</xsl:stylesheet>
