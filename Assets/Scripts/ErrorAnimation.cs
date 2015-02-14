using UnityEngine;
using System.Collections;

public class ErrorAnimation : MonoBehaviour {

	// Use this for initialization
	void Start () {
        StartCoroutine(animation());
	}
	
    private IEnumerator animation() {
        Vector2[] uvs = new Vector2[4];
        Mesh mesh = GetComponent<MeshFilter>().mesh;
        int frame = 0;

        uvs[0] = new Vector2(frame/8.0f, 0);
        uvs[1] = new Vector2((frame + 1)/8.0f, 1);
        uvs[2] = new Vector2((frame + 1)/8.0f, 0);
        uvs[3] = new Vector2(frame/8.0f, 1);
        
        mesh.uv = uvs;
        
        yield return new WaitForSeconds(UnityEngine.Random.value);

        frame++;

        while (true) {
            uvs[0] = new Vector2(frame/8.0f, 0);
            uvs[1] = new Vector2((frame + 1)/8.0f, 1);
            uvs[2] = new Vector2((frame + 1)/8.0f, 0);
            uvs[3] = new Vector2(frame/8.0f, 1);

            mesh.uv = uvs;

            if (frame == 0) {
                yield return new WaitForSeconds(1);
            } else {
                yield return new WaitForSeconds(0.02f);
            }

            frame = (frame + 1) % 8;
        }
    }
}
