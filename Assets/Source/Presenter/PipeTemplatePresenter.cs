using System;
using UnityEngine;

[RequireComponent(typeof(PipeTemplateView))]
public class PipeTemplatePresenter : Presenter
{
    [SerializeField] private Fuel _fuelType;

    public new PipeTemplate Model => base.Model as PipeTemplate;
    public new PipeTemplateView View => base.View as PipeTemplateView;

    public event Action<PipeTemplate> Inited;

    public Fuel FuelType => _fuelType;

    private void Awake()
    {
        PipePiecePresenter[] pipePiecePresenters = GetComponentsInChildren<PipePiecePresenter>();

        PipePiece[] pipePieces = new PipePiece[pipePiecePresenters.Length];

        for (int i = 0; i < pipePieces.Length; i++)
        {
            pipePiecePresenters[i].Init(new PipePiece(pipePiecePresenters[i].transform.position, transform.position, FuelType));
            pipePieces[i] = pipePiecePresenters[i].Model;
        }

        Init(new PipeTemplate(pipePieces, FuelType));
        View.Init(_fuelType);

        Inited?.Invoke(Model);
    }
}
