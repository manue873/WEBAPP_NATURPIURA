using System.Security.Cryptography;

namespace WEBAPP_NATURPIURA.Servicios
{
    public class Encriptacion
    {
        /// <summary>
        /// Encriptacion de la clave mediante AES, la Key y la Iv se almacenan dentro de la misma clave
        /// </summary>
        /// <param name="plainText">recibe la clave ingresada en el formulario de inicio de session en formato String</param>
        /// <returns>Retorna la clave encriptada que se almacenara </returns>
        public static byte[] Encrypt(string plainText)
        {
            //Usa la Libreria de AES Cifrar
            using (Aes aesAlg = Aes.Create())
            {
                // Generar una key y un vector de inicialización (IV) para el cifrado
                aesAlg.GenerateKey();
                aesAlg.GenerateIV();
                // Crear el transformador de cifrado utilizando la key y el IV generados
                ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);
                // Crear un MemoryStream para almacenar los datos cifrados
                using (MemoryStream msEncrypt = new MemoryStream())
                {
                    // Escribir la key y el IV en el stream antes de los datos cifrados
                    msEncrypt.Write(aesAlg.Key, 0, aesAlg.Key.Length);
                    msEncrypt.Write(aesAlg.IV, 0, aesAlg.IV.Length);
                    // Crear un CryptoStream para realizar el cifrado en modo de escritura
                    using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                    {
                        // Escribir el texto plano en el CryptoStream, que se cifra automáticamente
                        swEncrypt.Write(plainText);
                    }
                    // Devolver el arreglo de bytes que contiene la Key, el IV y los datos cifrados
                    return msEncrypt.ToArray();
                }
            }
        }
        /// <summary>
        ///  Desencriptacion de la clave generada por el metodo <see cref="Encrypt(string)"/> 
        /// </summary>
        /// <param name="cipherText">texto cifrado</param>
        /// <returns>Retorna la clave decifrada</returns>
        public static string Decrypt(byte[] cipherText)
        {
            //creanos un MemoryStream para almacenar la clave que vamos a trabajar
            using (MemoryStream msDecrypt = new MemoryStream(cipherText))
            {
                // Creamos los arreglos para almacenar la clave (Key) y el vector de inicialización (IV).
                byte[] key = new byte[32];
                byte[] iv = new byte[16];
                // Leemos la clave y el IV desde el inicio del arreglo de bytes.
                msDecrypt.Read(key, 0, key.Length);
                msDecrypt.Read(iv, 0, iv.Length);

                //usamos AES desencriptar 
                using (Aes aesAlg = Aes.Create())
                {
                    //le indicamos cual es la key y la iv y asignamos
                    aesAlg.Key = key;
                    aesAlg.IV = iv;

                    // Instanciamos en una varianle y le enviamos la key y la iv para la desencriptacion
                    ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

                    // Desencriptamos los datos utilizando un CryptoStream.
                    using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                    {
                        // Retornamos el texto desencriptado en formato de String 
                        return srDecrypt.ReadToEnd();
                    }
                }
            }
        }
    }

}
