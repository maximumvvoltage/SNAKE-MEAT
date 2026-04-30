using UnityEngine;

public class FloatingIsland : MonoBehaviour
{
    [Header("Animation Clips")]
    public AnimationClip normalClip;
    public AnimationClip fastClip;
    public AnimationClip slowClip;

    private void Start()
    {
        ApplyClipToTagged("Normal", normalClip);
        ApplyClipToTagged("Fast", fastClip);
        ApplyClipToTagged("Slow", slowClip);
    }

    private void ApplyClipToTagged(string tag, AnimationClip clip)
    {
        GameObject[] objects = GameObject.FindGameObjectsWithTag(tag);

        foreach (GameObject obj in objects)
        {
            Animation anim = obj.GetComponent<Animation>();

            anim.AddClip(clip, clip.name);
            anim.clip = clip;
            anim.Play(clip.name);
        }
    }
}