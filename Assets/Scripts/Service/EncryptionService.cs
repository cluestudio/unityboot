using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public interface EncryptionService {
    string Encrypt(string key, string ToEncrypt);
    string Decrypt(string key, string cypherString);
}