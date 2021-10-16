Imports System.Management
Imports System.Text
Imports System.Security.Cryptography

Public Class AWSLicenseKey
    Class LicenseKey
        Public Shared Function GetHardwareKey() As String
            Dim key As String = "Dia-Analysis!!@#@$()*&#^%(#()@_@&#$^$%)@_@_@986544__*#*$&#&^@)@)&)#&)$&)$&)$&)$"
            Return key
        End Function

        Public Shared Function GetHardwareId() As String
            Dim HadId As String = String.Format("{0}#{1}#{2}", GetProcessorId(), GetMotherboardID(), GetDiskVolumeSerialNumber())
            Return HadId
        End Function

        Public Shared Function GetActivtionSecureKeyword(ByVal Licensetype As Integer) As String
            Dim key As String = ""

            If Licensetype = 1 Then
                key = "$&)&$)&$)&#))(&#)(#&)#&)&0333333#^%(#()@_@&#$^$%)@_@_@__*#*$&#&^@)@)&)#&)$&)$&)$&)$"
            ElseIf Licensetype = 2 Then
                key = "!!@33333(#&$^^@&@*$*#*(@)*(#(#()**$@*$@*$@$@)()#@$()*&333333#^%(#()@_@&#$^$%)@_@_@__*#*$&#&^@)@)&)#&)$&)$&)$&)$"
            End If

            Return key
        End Function

        Public Shared Function GetDiskVolumeSerialNumber() As String
            Try
                Dim serialNo As String = ""
                Dim searcher As ManagementObjectSearcher = New ManagementObjectSearcher("SELECT * FROM Win32_DiskDrive")

                For Each info As ManagementObject In searcher.[Get]()
                    serialNo = info("SerialNumber").ToString()
                Next

                Return serialNo
            Catch
                Return String.Empty
            End Try
        End Function

        Public Shared Function GetProcessorId() As String
            Try
                Dim _mbs As ManagementObjectSearcher = New ManagementObjectSearcher("Select ProcessorId From Win32_processor")
                Dim _mbsList As ManagementObjectCollection = _mbs.[Get]()
                Dim _id As String = String.Empty

                For Each _mo As ManagementObject In _mbsList
                    _id = _mo("ProcessorId").ToString()
                    Exit For
                Next

                Return _id
            Catch
                Return String.Empty
            End Try
        End Function

        Public Shared Function GetMotherboardID() As String
            Try
                Dim _mbs As ManagementObjectSearcher = New ManagementObjectSearcher("Select SerialNumber From Win32_BaseBoard")
                Dim _mbsList As ManagementObjectCollection = _mbs.[Get]()
                Dim _id As String = String.Empty

                For Each _mo As ManagementObject In _mbsList
                    _id = _mo("SerialNumber").ToString()
                    Exit For
                Next

                Return _id
            Catch
                Return String.Empty
            End Try
        End Function

        Public Shared Function Encrypt(ByVal toEncrypt As String, ByVal useHashing As Boolean, ByVal key As String) As String
            Dim EncryptString As String = ""

            Try
                Dim keyArray As Byte()
                Dim toEncryptArray As Byte() = UTF8Encoding.UTF8.GetBytes(toEncrypt)

                If useHashing Then
                    Dim hashmd5 As MD5CryptoServiceProvider = New MD5CryptoServiceProvider()
                    keyArray = hashmd5.ComputeHash(UTF8Encoding.UTF8.GetBytes(key))
                    hashmd5.Clear()
                Else
                    keyArray = UTF8Encoding.UTF8.GetBytes(key)
                End If

                Dim tdes As TripleDESCryptoServiceProvider = New TripleDESCryptoServiceProvider()
                tdes.Key = keyArray
                tdes.Mode = CipherMode.ECB
                tdes.Padding = PaddingMode.PKCS7
                Dim cTransform As ICryptoTransform = tdes.CreateEncryptor()
                Dim resultArray As Byte() = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length)
                tdes.Clear()
                EncryptString = Convert.ToBase64String(resultArray, 0, resultArray.Length)
            Catch ex As Exception
                EncryptString = ""
            End Try

            Return EncryptString
        End Function

        Public Shared Function Decrypt(ByVal cipherString As String, ByVal useHashing As Boolean, ByVal key As String) As String
            Dim DecryptString As String = ""

            Try
                Dim keyArray As Byte()
                Dim toEncryptArray As Byte() = Convert.FromBase64String(cipherString)

                If useHashing Then
                    Dim hashmd5 As MD5CryptoServiceProvider = New MD5CryptoServiceProvider()
                    keyArray = hashmd5.ComputeHash(UTF8Encoding.UTF8.GetBytes(key))
                    hashmd5.Clear()
                Else
                    keyArray = UTF8Encoding.UTF8.GetBytes(key)
                End If

                Dim tdes As TripleDESCryptoServiceProvider = New TripleDESCryptoServiceProvider()
                tdes.Key = keyArray
                tdes.Mode = CipherMode.ECB
                tdes.Padding = PaddingMode.PKCS7
                Dim cTransform As ICryptoTransform = tdes.CreateDecryptor()
                Dim resultArray As Byte() = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length)
                tdes.Clear()
                DecryptString = UTF8Encoding.UTF8.GetString(resultArray)
            Catch ex As Exception
                DecryptString = ""
            End Try

            Return DecryptString
        End Function

        Public Shared Function GenerateHardwareKey() As String
            Dim LicKey As String = GetHardwareId()
            LicKey = Encrypt(LicKey, True, GetHardwareKey())
            Return LicKey
        End Function

        Public Shared Function GenerateActivationKey(ByVal HardwareKey As String, ByVal Licensetype As Integer, ByVal ProductType As Integer) As String
            Dim ActiveLicKey As String = ""
            Dim License As String = "Demo"

            If Licensetype = 2 Then
                License = "Live"
            End If

            Dim Product As String = ""

            If ProductType = 1 Then
                Product = "Basic_License"
            End If

            If ProductType = 2 Then
                Product = "Standard_License"
            End If

            If ProductType = 3 Then
                Product = "Premium_License"
            End If

            Dim ExpireTime As String = DateTime.Now.AddDays(10).ToShortDateString().ToString()
            HardwareKey = String.Format("{0}|{1}|{2}|{3}|{4}", Product, License, ExpireTime, HardwareKey, ProductType)
            ActiveLicKey = Encrypt(HardwareKey, True, GetActivtionSecureKeyword(Licensetype))
            Return ActiveLicKey
        End Function

        Public Shared Function HardwareKeyMatch(ByVal HardwareKey As String, ByVal ActivationKey As String, ByRef ExpireDate As String, ByRef ActivationType As String, ByRef ProductType As Integer) As Boolean
            Dim result As Boolean = False
            ProductType = 1

            Try
                Dim Matchresult As String = Decrypt(ActivationKey, True, GetActivtionSecureKeyword(1))
                Dim GetHardwareKey As String = ""

                If Matchresult.Length = 0 Then
                    Matchresult = Decrypt(ActivationKey, True, GetActivtionSecureKeyword(2))
                End If

                Dim strArrays As String()

                If Matchresult.Length > 0 Then
                    strArrays = Microsoft.VisualBasic.Strings.Split(Matchresult, "|", -1, Microsoft.VisualBasic.CompareMethod.Binary)
                    ActivationType = strArrays(1)
                    ExpireDate = strArrays(2)
                    GetHardwareKey = strArrays(3)

                    If strArrays.Length > 3 Then
                        ProductType = Convert.ToInt32(strArrays(4))
                    End If

                    If HardwareKey = GetHardwareKey.TrimEnd() Then
                        result = True
                    End If
                End If

            Catch ex As Exception
                result = False
            End Try

            Return result
        End Function
    End Class

End Class
