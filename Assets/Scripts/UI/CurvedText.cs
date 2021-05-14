using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

[ExecuteInEditMode]
public class CurvedText : Text
{

	[Header("Curving")]
	[SerializeField] private float radius = 0.5f;
	[SerializeField] private float wrapAngle = 360;
	[SerializeField] private float scaleFactor = 100;
	[SerializeField] private bool scaleCharacters = false;
	[SerializeField] private bool autoSizeWidth = false;

	private float circumference;

#if UNITY_EDITOR

	protected override void OnValidate()
	{

		base.OnValidate();

		if (radius <= 0)
		{

			radius = 0.001f;

		}

		if (scaleFactor <= 0)
		{

			scaleFactor = 0.001f;

		}

		circumference = 2 * Mathf.PI * radius * scaleFactor;

	}

#else

    protected override void Start()
    {

		base.Start();

		circumference = 2 * Mathf.PI * radius * scaleFactor;

	}

#endif

	protected override void OnPopulateMesh(VertexHelper vertexHelper)
	{

		base.OnPopulateMesh(vertexHelper);

		List<UIVertex> stream = new List<UIVertex>();

		vertexHelper.GetUIVertexStream(stream);

		if (scaleCharacters)
        {

			for (int i = 0; i < stream.Count; i++)
			{

				UIVertex v = stream[i];

				float percentCircumference = v.position.x / circumference;

				Vector3 offset = Quaternion.Euler(0, 0, -percentCircumference * 360) * Vector3.up;

				v.position = offset * radius * scaleFactor + offset * v.position.y;

				v.position += Vector3.down * radius * scaleFactor;

				stream[i] = v;

			}

		}
        else
        {

			for (int i = 0; i < stream.Count; i += 6)
			{

				Vector3 quadCenter = Vector3.zero;

				for (int j = 0; j < 6; j++)
				{

					quadCenter += stream[i + j].position;

				}

				quadCenter /= 6;

				float quadWidth = stream[i + 1].position.x - stream[i + 0].position.x;

				float quadHeight = stream[i + 5].position.y - stream[i + 4].position.y;

				Vector3[] offsets = {
					new Vector3(-quadWidth / 2, quadHeight / 2, 0),
					new Vector3(quadWidth / 2, quadHeight / 2, 0),
					new Vector3(quadWidth / 2, -quadHeight / 2, 0),
					new Vector3(quadWidth / 2, -quadHeight / 2, 0),
					new Vector3(-quadWidth / 2, -quadHeight / 2, 0),
					new Vector3(-quadWidth / 2, quadHeight / 2, 0)
				};

				for (int j = 0; j < 6; j++)
				{

					UIVertex vertex = stream[i + j];

					/*Color[] colors = { Color.red, Color.yellow, Color.green, Color.cyan, Color.blue, Color.black };

					for (int k = 0; k < 6; k++)
					{

						if (((i + j) - k) % 6 == 0)
						{

							vertex.color = colors[k];

							break;

						}

					}*/

					float percentCircumference = quadCenter.x / circumference;

					Quaternion rotator = Quaternion.Euler(0, 0, -percentCircumference * 360);

					Vector3 offset = rotator * Vector3.up;

					vertex.position = offset * radius * scaleFactor + offset * quadCenter.y;

					vertex.position += rotator * offsets[j];

					vertex.position += Vector3.down * radius * scaleFactor;

					stream[i + j] = vertex;

				}

			}

		}
		
		vertexHelper.AddUIVertexTriangleStream(stream);

	}

	void Update()
	{

		if (autoSizeWidth)
        {

			rectTransform.sizeDelta = new Vector2(circumference * wrapAngle / 360, rectTransform.sizeDelta.y);

		}

	}

}
