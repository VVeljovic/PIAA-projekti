using System;
using System.Collections.Generic;
using System.Text;

namespace ShannonFano
{
    class BSTree
    {
        public BSTNode root;
        public int count = 0;
        public void Add(PomocnaStruktura[] niz)
        {
            if (root == null)
            {
                root = new BSTNode(niz);
                count++;
                Add(root, niz);
            }
        }
        public void Add(BSTNode a, PomocnaStruktura[] niz)
        {
            BSTNode ptr = a;
            if (ptr.key.Length<2)
                return;
            PomocnaStruktura[] niz1;
            PomocnaStruktura[] niz2;
            count++;
            ptr.Half(ptr.key, out niz1, out niz2);
            ptr.left = new BSTNode(niz1);
            ptr.right = new BSTNode(niz2);
            Add(ptr.left, ptr.left.key);
            Add(ptr.right, ptr.right.key);
        }
        public void Kodiranje()
        {
            Kodiranje(root, null);
        }
        public void Kodiranje(BSTNode p,string a)
        {
            string c;
            if (p == null)
            {

                //a = a.Substring(0, a.Length-1);
                return;

            }
            if (p.right == null && p.left == null)
            {
                p.key[0].kod += a;


            }
            // if (a != null)
            // c = a.Substring(0, a.Length);
            c = a;
            Kodiranje(p.left, a += "0");
            a = c;
            Kodiranje(p.right, a += "1");
        }
        public void visit()
        {
            visit(root);
        }
        public void visit(BSTNode ptr)
        {
            if (ptr == null)
                return;
            if (ptr.left == null && ptr.right == null)
                Console.WriteLine(ptr.key[0].slovo+" "+ ptr.key[0].freq +" "+ptr.key[0].kod);
            visit(ptr.left);
            visit(ptr.right);

        }

    }
}
