using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Kruskal : MonoBehaviour
{
    public void KruskalAlgorithm()
    {
        UnionFind uf = new UnionFind();

        int n = 7;

        List<Edge> list = new List<Edge>();
        list.Add(new Edge(1, 7, 12));
        list.Add(new Edge(1, 4, 28));
        list.Add(new Edge(1, 2, 67));
        list.Add(new Edge(1, 5, 17));
        list.Add(new Edge(2, 4, 24));
        list.Add(new Edge(2, 5, 62));
        list.Add(new Edge(3, 5, 20));
        list.Add(new Edge(3, 6, 37));
        list.Add(new Edge(4, 7, 13));
        list.Add(new Edge(5, 6, 45));
        list.Add(new Edge(5, 7, 73));

        // ������ ����� �������� �������� ����
        list = list.OrderBy(o => o.distance).ToList();

        // �� ������ ���Ե� �׷����� ������� ����
        int[] parent = new int[n];
        for (int i = 0; i < n; i++)
        {
            parent[i] = i;
        }

        // �Ÿ��� ���� 0���� �ʱ�ȭ
        int sum = 0;
        for (int i = 0; i < list.Count; i++)
        {
            // ������ �θ� ����Ű�� �ʴ� ���, �� ����Ŭ�� �߻����� ���� ���� ����
            if (!uf.FindParent(parent, list[i].node[0] - 1, list[i].node[1] - 1))
            {
                sum += list[i].distance;
                uf.UnionParent(parent, list[i].node[0] - 1, list[i].node[1] - 1);
            }
        }

        Console.WriteLine(sum);
    }
}

// ���� Ŭ����
class Edge
{
    public int[] node = new int[2];
    public int distance;

    public Edge(int a, int b, int distance)
    {
        node[0] = a;
        node[1] = b;
        this.distance = distance;
    }
}

