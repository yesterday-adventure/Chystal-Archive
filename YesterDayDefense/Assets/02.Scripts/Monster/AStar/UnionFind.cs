using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnionFind : MonoBehaviour
{
    public UnionFind()
    {
        //int[] parent = new int[11];
        //for (int i = 0; i <= 10; i++)
        //{
        //    parent[i] = i;
        //}

        //UnionParent(parent, 1, 2);
        //UnionParent(parent, 2, 3);
        //UnionParent(parent, 3, 4);
        //UnionParent(parent, 5, 6);
        //UnionParent(parent, 6, 7);
        //UnionParent(parent, 7, 8);
        //Console.WriteLine("1과 5가 연결되어있나요? " + FindParent(parent, 1, 5));
        //UnionParent(parent, 2, 8);
        //Console.WriteLine("1과 5가 연결되어있나요? " + FindParent(parent, 1, 5));
    }


    public int GetParent(int[] parent, int x)
    {
        if (parent[x] == x) return x;
        return parent[x] = GetParent(parent, parent[x]);
    }

    // 각 부모 노드를 합침
    public void UnionParent(int[] parent, int a, int b)
    {
        a = GetParent(parent, a);
        b = GetParent(parent, b);
        if (a < b) parent[b] = a;
        else parent[a] = b;
    }

    // 같은 부모 노드를 가지는지 확인
    public bool FindParent(int[] parent, int a, int b)
    {
        a = GetParent(parent, a);
        b = GetParent(parent, b);
        if (a == b) return true;
        else return false;
    }

}
