using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TetrisBlock : MonoBehaviour
{
    public Vector3 rotationPoint = Vector3.zero;

    public float timeToFall = 0.8f;
    private float timeSinceLastMovementDown = 0;

    private static int width = 10;
    private static int height = 20;
    private float rotationAngle = 90;

    private static Transform[,] grid = new Transform[width, height];

    private void Start()
    {
        if (!ValidMovement())
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(0);
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            transform.position += Vector3.left;
            if (!ValidMovement())
            {
                transform.position -= Vector3.left;
            }
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            transform.position += Vector3.right;
            if (!ValidMovement())
            {
                transform.position -= Vector3.right;
            }
        }
        else if (Input.GetKeyDown(KeyCode.Space))
        {
            transform.RotateAround(transform.TransformPoint(rotationPoint), Vector3.forward, rotationAngle);
            if (!ValidMovement())
            {
                transform.RotateAround(transform.TransformPoint(rotationPoint), Vector3.forward, -rotationAngle);
            }
        }

        if (Time.time - timeSinceLastMovementDown > (Input.GetKey(KeyCode.S) ? timeToFall / 10 : timeToFall))
        {
            transform.position += Vector3.down;
            timeSinceLastMovementDown = Time.time;
            if (!ValidMovement())
            {
                transform.position -= Vector3.down;
                this.enabled = false;
                AddToGrid();
                CheckForLine();
                FindObjectOfType<Spawner>().SpawnBlock();
            }
        }
    }

    public void CheckForLine()
    {
        for (int i = height - 1; i >= 0; i--)
        {
            if (LineIsComplete(i))
            {
                DeleteLine(i);
                RowDown(i);
            }
        }
    }

    public void RowDown(int column)
    {
        for (int i = column; i < height; i++)
        {
            for (int j = 0; j < width; j++)
            {
                if (grid[j, i] != null)
                {
                    grid[j, i - 1] = grid[j, i];
                    grid[j, i] = null;
                    grid[j, i - 1].position += Vector3.down;
                }
            }
        }
    }

    private void DeleteLine(int column)
    {
        for (int i = 0; i < width; i++)
        {
            Destroy(grid[i, column].gameObject);
            grid[i, column] = null;
        }
    }

    private bool LineIsComplete(int column)
    {
        for (int i = 0; i < width; i++)
        {
            if (grid[i, column] == null)
            {
                return false;
            }
        }
        return true;
    }

    public void AddToGrid()
    {
        foreach (Transform child in transform)
        {
            int roundedX = Mathf.RoundToInt(child.position.x);
            int roundedY = Mathf.RoundToInt(child.position.y);
            grid[roundedX, roundedY] = child;
        }
    }

    public bool ValidMovement()
    {
        foreach (Transform child in transform)
        {
            int roundedX = Mathf.RoundToInt(child.position.x);
            int roundedY = Mathf.RoundToInt(child.position.y);

            if (roundedX < 0 || roundedX >= width || roundedY < 0 || roundedY >= height || grid[roundedX, roundedY] != null)
            {
                return false;
            }
        }
        return true;
    }
}