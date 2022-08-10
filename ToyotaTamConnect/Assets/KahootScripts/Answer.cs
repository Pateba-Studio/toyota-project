[System.Serializable]
public class Answer
{
    public string jawaban;
    public bool isTrue;

    public Answer(string jawaban, bool isTrue)
    {
        this.jawaban = jawaban;
        this.isTrue = isTrue;
    }
}
