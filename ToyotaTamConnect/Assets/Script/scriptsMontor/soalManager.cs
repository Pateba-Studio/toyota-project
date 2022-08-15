using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class soalManager : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI plat, leftAns, rightAns, timer, soal;
    [SerializeField] GameObject obstacle, soalPanel;
    List<int> list = new List<int>();
    List<string> listJawabPlat, listPlat, listSoal, listJawaban, fakeJawaban;
    [SerializeField] uiManager ui;
    int i, index;
    float timeLeft = 5;
    bool counting, bukaSoal;
    // Start is called before the first frame update
    void Start()
    {
        //listJawabPlat = new List<string>() {"Brebes", "Kendal", "Pati", "Cilacap", "Wonosobo", "Bantul", "Surakarta"};
        //listPlat = new List<string>() { "G 810 K", "H 1945 AW", "K 270 L", "R 154 U", "AA 1517 EU", "AB 170 D", "AD 420 E"};
        //listSoal = new List<string>() { 
        //    "Kota batik merupakan sebutan untuk daerah...",
        //    "Museum Kereta Api Ambarawa terletak di kabupaten...",
        //    "Kota Ukir merupakan julukan untuk kabupaten...",
        //    "Tempe Mendoan merupakan makanan khas kabupaten...",
        //    "Candi Borobudur terletak di kabupaten...",
        //    "Gudeg merupakan makanan khas daerah...",
        //    "Candi Cetho terletak di kabupaten..." };
        //listJawaban = new List<string>() { "Pekalongan", "Semarang", "Jepara", "Banyumas", "Magelang", "Yogyakarta", "Karanganyar" };
        //fakeJawaban = new List<string>() { "Pemalang", "Demak", "Kudus", "Purbalingga", "Kebumen", "Sleman", "Klaten" };

        //listJawabPlat = new List<string>() { "", "", "", "", "" };
        //listPlat = new List<string>() { "", "", "", "", "" };
        listSoal = new List<string>() {
            "Menggunakan waktu istirahat makan siang melebihi waktu yang ditentukan, Apakah termasuk tindakan korupsi?",
            "Menggunakan mobil operasional kantor untuk kebutuhan pribadi, Apakah termasuk tindakan korupsi?",
            "Menerima hadiah dari calon konsumen, Apakah termasuk tindakan korupsi?",
            "Melaporkan hasil temuan transaksi penggunaan uang kantor untuk pribadi, Apakah termasuk tindakan korupsi?",
            "Melaporkan kejadian suap antara calon konsumen dengan sales, Apakah termasuk tindakan korupsi?"
        };
        listJawaban = new List<string>() { "Ya", "Ya", "Ya", "Tidak", "Tidak" };
        fakeJawaban = new List<string>() { "Tidak", "Tidak", "Tidak", "Ya", "Ya"};

        for (int n = 0; n < 5; n++)    //  Populate list
        {
            list.Add(n);
        }
        StartCoroutine(NumberGen());
    }
    // Update is called once per frame
    void Update()
    {
        if (counting)
        {
            timeLeft -= 1 * Time.deltaTime;
            timer.text = timeLeft.ToString("0");
        }
    }
    IEnumerator NumberGen()
    {
        while (list.Count!=0)
        {
            index = Random.Range(0, list.Count); //mengambil elemen acak dari list
            i = list[index];    //  i = the number that was randomly picked
            list.RemoveAt(index); //menghapus angka yang sudah dipilih secara acak dari list, supaya tidak terulang

            //plat.text = listPlat[i];
            //bukaSoalCok = false;
            //if (Random.Range(0, 2) == 1)
            //    leftTrue();
            //else
            //    rightTrue();
            //counting = true;
            //foreach (int integer in list)
            //{
            //    Debug.Log(integer.ToString());
            //}
            yield return new WaitForSeconds(7);
            soal.text = listSoal[i];
            plat.text = "";
            ui.Pause();
            soalPanel.SetActive(true);
            bukaSoal = true;
            if (Random.Range(0, 2) == 1)
                leftTrue();
            else
                rightTrue();
            counting = true;
            yield return new WaitForSeconds(7);
        }
        //kondisi kemenangan disini
        Debug.Log("selesai");
        Debug.Log(index);
        foreach (int integer in list)
            Debug.Log(integer);
        
        ui.GameOverActivated();
    }
    private void leftTrue()
    {
        //while (i == index)
        //{
        //    index = Random.Range(0, listJawabPlat.Count);
        //}
        if (bukaSoal)
        {
            rightAns.text = fakeJawaban[i];
            leftAns.text = listJawaban[i];
        }
        //else if(!bukaSoalCok)
        //{
        //    rightAns.text = listJawabPlat[index];
        //    leftAns.text = listJawabPlat[i];
        //}
        StartCoroutine(spawnObs(1.4f));
    }
    private void rightTrue()
    {
        //while (i == index)
        //{
        //    index = Random.Range(0, listJawabPlat.Count);
        //}
        if (bukaSoal)
        {
            rightAns.text = listJawaban[i];
            leftAns.text = fakeJawaban[i];
        }
        //else if(!bukaSoalCok)
        //{
        //    rightAns.text = listJawabPlat[i];
        //    leftAns.text = listJawabPlat[index];
        //}
        StartCoroutine(spawnObs(-1.4f));
    }
    IEnumerator spawnObs(float x)
    {
        yield return new WaitForSeconds(5);
        soal.text = " ";
        Vector3 pos = new Vector3(x, obstacle.transform.position.y, obstacle.transform.position.z);
        Instantiate(obstacle, pos, obstacle.transform.rotation);
        counting = false;
        timeLeft = 5;
    }
}