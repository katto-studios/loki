using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NUnit.Framework;
using UnityEngine.TestTools;

public class Sort_Test{
    [Test]
    public void MergeSortOneToTen(){
        int[] correctArray = new[]{0, 1, 2, 3, 4, 5, 6, 7, 8, 9};
        int[] randomArray = new[]{0, 3, 1, 9, 2, 6, 5, 7, 4, 8};
        MergeSort(randomArray, 0, randomArray.Length - 1);
        
        Assert.AreEqual(correctArray, randomArray);
    }

    [Test]
    public void BubbleSortOneToTen(){
        int[] correctArray = new[]{0, 1, 2, 3, 4, 5, 6, 7, 8, 9};
        int[] randomArray = new[]{0, 3, 1, 9, 2, 6, 5, 7, 4, 8};
        BubbleSort(randomArray);
        
        Assert.AreEqual(correctArray, randomArray);
    }

    private void BubbleSort(int[] _in){
        bool sorted = false;
        while (!sorted){
            sorted = true;
            for (int count = 1; count < _in.Length; count++){
                if (_in[count] < _in[count - 1]){
                    _in[count] ^= _in[count - 1];
                    _in[count - 1] ^= _in[count];
                    _in[count] ^= _in[count - 1];
                    sorted = false;
                }
            }
        }
    }

    private void MergeSort(int[] _in, int _left, int _right){
        if (_left < _right){
            int partition = (_left + _right) / 2;
            
            MergeSort(_in, _left, partition);
            MergeSort(_in, partition + 1, _right);

            Merge(_in, _left, partition, _right);
        }
    }

    private void Merge(int[] _in, int _left, int _middle, int _right){
        int i, j, k;
        int n1 = _middle - _left + 1;
        int n2 = _right - _middle;
        
        //temp arrays
        int[] tLeft = new int[n1];
        int[] tRight = new int[n2];

        for (i = 0; i < n1; i++){
            tLeft[i] = _in[_left + i];
        }

        for (j = 0; j < n2; j++){
            tRight[j] = _in[_middle + 1 + j];
        }

        i = 0;
        j = 0;
        k = _left;
        while (i < n1 && j < n2){
            _in[k++] = tLeft[i] < tRight[j] ? tLeft[i++] : tRight[j++];
        }

        while (i < n1) _in[k++] = tLeft[i++];
        while (j < n2) _in[k++] = tRight[j++];
    }
}