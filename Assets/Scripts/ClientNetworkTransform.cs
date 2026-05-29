using Unity.Netcode.Components;
using UnityEngine;

[DisallowMultipleComponent]
public class ClientNetworkTransform : NetworkTransform
{
    // Эта строчка говорит: "Разрешить клиентам двигать свой объект"
    protected override bool OnIsServerAuthoritative()
    {
        return false;
    }
}