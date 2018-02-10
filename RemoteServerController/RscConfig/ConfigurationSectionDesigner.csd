<?xml version="1.0" encoding="utf-8"?>
<configurationSectionModel xmlns:dm0="http://schemas.microsoft.com/VisualStudio/2008/DslTools/Core" dslVersion="1.0.0.0" Id="60b7b06b-dd35-4f6d-be4e-c242bc44b06f" namespace="RscConfig" xmlSchemaNamespace="urn:RscConfig" xmlns="http://schemas.microsoft.com/dsltools/ConfigurationSectionDesigner">
  <typeDefinitions>
    <externalType name="String" namespace="System" />
    <externalType name="Boolean" namespace="System" />
    <externalType name="Int32" namespace="System" />
    <externalType name="Int64" namespace="System" />
    <externalType name="Single" namespace="System" />
    <externalType name="Double" namespace="System" />
    <externalType name="DateTime" namespace="System" />
    <externalType name="TimeSpan" namespace="System" />
  </typeDefinitions>
  <configurationElements>
    <configurationSection name="StaticConfiguration" codeGenOptions="Singleton, XmlnsProperty" xmlSectionName="staticConfiguration">
      <elementProperties>
        <elementProperty name="General" isRequired="false" isKey="false" isDefaultCollection="false" xmlName="general" isReadOnly="true">
          <type>
            <configurationElementMoniker name="/60b7b06b-dd35-4f6d-be4e-c242bc44b06f/General" />
          </type>
        </elementProperty>
        <elementProperty name="Network" isRequired="false" isKey="false" isDefaultCollection="false" xmlName="network" isReadOnly="true">
          <type>
            <configurationElementMoniker name="/60b7b06b-dd35-4f6d-be4e-c242bc44b06f/Network" />
          </type>
        </elementProperty>
      </elementProperties>
    </configurationSection>
    <configurationSection name="DynamicConfiguration" codeGenOptions="Singleton, XmlnsProperty" xmlSectionName="dynamicConfiguration">
      <elementProperties>
        <elementProperty name="Service" isRequired="false" isKey="false" isDefaultCollection="false" xmlName="service" isReadOnly="true">
          <type>
            <configurationElementMoniker name="/60b7b06b-dd35-4f6d-be4e-c242bc44b06f/Service" />
          </type>
        </elementProperty>
        <elementProperty name="Security" isRequired="false" isKey="false" isDefaultCollection="false" xmlName="security" isReadOnly="true">
          <type>
            <configurationElementMoniker name="/60b7b06b-dd35-4f6d-be4e-c242bc44b06f/Security" />
          </type>
        </elementProperty>
      </elementProperties>
    </configurationSection>
    <configurationElement name="General">
      <attributeProperties>
        <attributeProperty name="QuitToken" isRequired="true" isKey="false" isDefaultCollection="false" xmlName="quitToken" isReadOnly="true">
          <type>
            <externalTypeMoniker name="/60b7b06b-dd35-4f6d-be4e-c242bc44b06f/String" />
          </type>
        </attributeProperty>
      </attributeProperties>
    </configurationElement>
    <configurationElement name="Network">
      <attributeProperties>
        <attributeProperty name="Host" isRequired="true" isKey="false" isDefaultCollection="false" xmlName="host" isReadOnly="true">
          <type>
            <externalTypeMoniker name="/60b7b06b-dd35-4f6d-be4e-c242bc44b06f/String" />
          </type>
        </attributeProperty>
        <attributeProperty name="Port" isRequired="true" isKey="false" isDefaultCollection="false" xmlName="port" isReadOnly="true">
          <type>
            <externalTypeMoniker name="/60b7b06b-dd35-4f6d-be4e-c242bc44b06f/Int32" />
          </type>
        </attributeProperty>
        <attributeProperty name="UseSSL" isRequired="true" isKey="false" isDefaultCollection="false" xmlName="useSSL" isReadOnly="true" defaultValue="false">
          <type>
            <externalTypeMoniker name="/60b7b06b-dd35-4f6d-be4e-c242bc44b06f/Boolean" />
          </type>
        </attributeProperty>
        <attributeProperty name="CertificateThumbprint" isRequired="false" isKey="false" isDefaultCollection="false" xmlName="certificateThumbprint" isReadOnly="true">
          <type>
            <externalTypeMoniker name="/60b7b06b-dd35-4f6d-be4e-c242bc44b06f/String" />
          </type>
        </attributeProperty>
        <attributeProperty name="CrossDomainScriptAccessEnabled" isRequired="true" isKey="false" isDefaultCollection="false" xmlName="crossDomainScriptAccessEnabled" isReadOnly="true" defaultValue="true">
          <type>
            <externalTypeMoniker name="/60b7b06b-dd35-4f6d-be4e-c242bc44b06f/Boolean" />
          </type>
        </attributeProperty>
      </attributeProperties>
    </configurationElement>
    <configurationElement name="Service">
      <attributeProperties>
        <attributeProperty name="StatusChangeTimeout" isRequired="true" isKey="false" isDefaultCollection="false" xmlName="statusChangeTimeout" isReadOnly="true" defaultValue="5000">
          <type>
            <externalTypeMoniker name="/60b7b06b-dd35-4f6d-be4e-c242bc44b06f/Int32" />
          </type>
        </attributeProperty>
      </attributeProperties>
      <elementProperties>
        <elementProperty name="AllowedServiceCollection" isRequired="false" isKey="false" isDefaultCollection="false" xmlName="allowedServiceCollection" isReadOnly="true">
          <type>
            <configurationElementCollectionMoniker name="/60b7b06b-dd35-4f6d-be4e-c242bc44b06f/AllowedServiceCollection" />
          </type>
        </elementProperty>
      </elementProperties>
    </configurationElement>
    <configurationElementCollection name="AllowedServiceCollection" xmlItemName="allowedService" codeGenOptions="Indexer, AddMethod, RemoveMethod, GetItemMethods">
      <itemType>
        <configurationElementMoniker name="/60b7b06b-dd35-4f6d-be4e-c242bc44b06f/AllowedService" />
      </itemType>
    </configurationElementCollection>
    <configurationElement name="AllowedService">
      <attributeProperties>
        <attributeProperty name="Name" isRequired="true" isKey="true" isDefaultCollection="false" xmlName="name" isReadOnly="true">
          <type>
            <externalTypeMoniker name="/60b7b06b-dd35-4f6d-be4e-c242bc44b06f/String" />
          </type>
        </attributeProperty>
        <attributeProperty name="SecurityProfile" isRequired="true" isKey="false" isDefaultCollection="false" xmlName="securityProfile" isReadOnly="true">
          <type>
            <externalTypeMoniker name="/60b7b06b-dd35-4f6d-be4e-c242bc44b06f/String" />
          </type>
        </attributeProperty>
        <attributeProperty name="AllowStart" isRequired="true" isKey="false" isDefaultCollection="false" xmlName="allowStart" isReadOnly="true">
          <type>
            <externalTypeMoniker name="/60b7b06b-dd35-4f6d-be4e-c242bc44b06f/Boolean" />
          </type>
        </attributeProperty>
        <attributeProperty name="AllowStop" isRequired="true" isKey="false" isDefaultCollection="false" xmlName="allowStop" isReadOnly="true">
          <type>
            <externalTypeMoniker name="/60b7b06b-dd35-4f6d-be4e-c242bc44b06f/Boolean" />
          </type>
        </attributeProperty>
        <attributeProperty name="AllowStatusCheck" isRequired="true" isKey="false" isDefaultCollection="false" xmlName="allowStatusCheck" isReadOnly="true">
          <type>
            <externalTypeMoniker name="/60b7b06b-dd35-4f6d-be4e-c242bc44b06f/Boolean" />
          </type>
        </attributeProperty>
      </attributeProperties>
    </configurationElement>
    <configurationElement name="Security">
      <elementProperties>
        <elementProperty name="SecurityProfileCollection" isRequired="false" isKey="false" isDefaultCollection="false" xmlName="securityProfileCollection" isReadOnly="true">
          <type>
            <configurationElementCollectionMoniker name="/60b7b06b-dd35-4f6d-be4e-c242bc44b06f/SecurityProfileCollection" />
          </type>
        </elementProperty>
      </elementProperties>
    </configurationElement>
    <configurationElementCollection name="SecurityProfileCollection" xmlItemName="securityProfile" codeGenOptions="Indexer, AddMethod, RemoveMethod, GetItemMethods">
      <itemType>
        <configurationElementMoniker name="/60b7b06b-dd35-4f6d-be4e-c242bc44b06f/SecurityProfile" />
      </itemType>
    </configurationElementCollection>
    <configurationElement name="SecurityProfile">
      <attributeProperties>
        <attributeProperty name="Name" isRequired="true" isKey="true" isDefaultCollection="false" xmlName="name" isReadOnly="true">
          <type>
            <externalTypeMoniker name="/60b7b06b-dd35-4f6d-be4e-c242bc44b06f/String" />
          </type>
        </attributeProperty>
        <attributeProperty name="CheckAPIKey" isRequired="true" isKey="false" isDefaultCollection="false" xmlName="checkAPIKey" isReadOnly="true">
          <type>
            <externalTypeMoniker name="/60b7b06b-dd35-4f6d-be4e-c242bc44b06f/Boolean" />
          </type>
        </attributeProperty>
        <attributeProperty name="CheckIPAddress" isRequired="true" isKey="false" isDefaultCollection="false" xmlName="checkIPAddress" isReadOnly="true">
          <type>
            <externalTypeMoniker name="/60b7b06b-dd35-4f6d-be4e-c242bc44b06f/Boolean" />
          </type>
        </attributeProperty>
      </attributeProperties>
      <elementProperties>
        <elementProperty name="AllowedAPIKeyCollection" isRequired="false" isKey="false" isDefaultCollection="false" xmlName="allowedAPIKeyCollection" isReadOnly="true">
          <type>
            <configurationElementCollectionMoniker name="/60b7b06b-dd35-4f6d-be4e-c242bc44b06f/AllowedAPIKeyCollection" />
          </type>
        </elementProperty>
        <elementProperty name="AllowedIPCollection" isRequired="false" isKey="false" isDefaultCollection="false" xmlName="allowedIPCollection" isReadOnly="true">
          <type>
            <configurationElementCollectionMoniker name="/60b7b06b-dd35-4f6d-be4e-c242bc44b06f/AllowedIPCollection" />
          </type>
        </elementProperty>
      </elementProperties>
    </configurationElement>
    <configurationElementCollection name="AllowedAPIKeyCollection" xmlItemName="allowedAPIKey" codeGenOptions="Indexer, AddMethod, RemoveMethod, GetItemMethods">
      <itemType>
        <configurationElementMoniker name="/60b7b06b-dd35-4f6d-be4e-c242bc44b06f/AllowedAPIKey" />
      </itemType>
    </configurationElementCollection>
    <configurationElement name="AllowedAPIKey">
      <attributeProperties>
        <attributeProperty name="Value" isRequired="true" isKey="true" isDefaultCollection="false" xmlName="value" isReadOnly="true">
          <type>
            <externalTypeMoniker name="/60b7b06b-dd35-4f6d-be4e-c242bc44b06f/String" />
          </type>
        </attributeProperty>
      </attributeProperties>
    </configurationElement>
    <configurationElement name="AllowedIP">
      <attributeProperties>
        <attributeProperty name="Value" isRequired="true" isKey="true" isDefaultCollection="false" xmlName="value" isReadOnly="true">
          <type>
            <externalTypeMoniker name="/60b7b06b-dd35-4f6d-be4e-c242bc44b06f/String" />
          </type>
        </attributeProperty>
      </attributeProperties>
    </configurationElement>
    <configurationElementCollection name="AllowedIPCollection" xmlItemName="allowedIP" codeGenOptions="Indexer, AddMethod, RemoveMethod, GetItemMethods">
      <itemType>
        <configurationElementMoniker name="/60b7b06b-dd35-4f6d-be4e-c242bc44b06f/AllowedIP" />
      </itemType>
    </configurationElementCollection>
  </configurationElements>
  <propertyValidators>
    <validators />
  </propertyValidators>
</configurationSectionModel>