using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICharacter
{
    void Move(Vector3 targetPosition);
    void Attack();
}
/*インターフェースを使う利点
コードの一貫性：
全てのキャラクター（例えば、戦士や弓兵）は ICharacter インターフェースを実装することにより、共通のインターフェースを持つことになります。これにより、キャラクターに対して同じ方法でアクセスし、操作できるようになります。

柔軟性：
異なる種類のキャラクターが異なる実装を持つ場合にも、統一された方法で操作できます。例えば、Warrior クラスや Archer クラスが ICharacter インターフェースを実装することで、それぞれのキャラクター特有の Move や Attack の実装が可能になります*/