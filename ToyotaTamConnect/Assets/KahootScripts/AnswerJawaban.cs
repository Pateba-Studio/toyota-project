[System.Serializable]
public class AnswerJawaban
{
    public string jawaban;
    public bool isTrue;

    public AnswerJawaban(string jawaban, bool isTrue)
    {
        this.jawaban = jawaban;
        this.isTrue = isTrue;
    }
}
