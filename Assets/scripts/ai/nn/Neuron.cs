using System;
// This represents the data structure used in the neural network
public class Neuron {
    private const float LEARNING_RATE = 0.05f;

    // sigmoid is costly, so calc 200 values for all neurons
    private float[] _sigmoid;
    private float _output;
    private Neuron[] _inputs;
    public float[] _weights;
    private float _error;
    public float bias;
    public float Error { get; set; }


    public float Output { get {return _output; } set { _output = value;} }

    public Neuron() {
        Random rand = new Random();
        bias = (float) (rand.NextDouble() * 10f);
    }

    public Neuron Copy() {
        Neuron n = new Neuron();
        if (_weights != null) {
            n._weights = new float[_weights.Length];
            n._inputs = new Neuron[_inputs.Length];
            for (int i = 0; i < _weights.Length; i++) {
                n._weights[i] = _weights[i];
            }
            for (int i = 0; i < _inputs.Length; i++) {
                n._inputs[i] = _inputs[i].Copy();
            }
        }

        return n;
    } 

    public Neuron(Neuron[] prev_inputs) {
        Random rand = new Random();
        _inputs = new Neuron[prev_inputs.Length];
        _weights = new float[prev_inputs.Length];

        for (int i = 0; i < _inputs.Length; i++) {
            _inputs[i] = prev_inputs[i];

            // random between -1 and 1
            _weights[i] = (float) (rand.NextDouble());
        }
        bias = (float) (rand.NextDouble() * 10f);
    }


    // 1/n sigma 1->n e^2  e = (i - a) i = ideal, a = actual
    // introduction to the math of neural networks
    // patrick winston nn mit courseware
    public void Train() {
        float delta = (1f - _output) * (1f + _output) * _error * LEARNING_RATE;
        for (int i = 0; i < _inputs.Length; i++) {
            _inputs[i].Error += _weights[i] * _error;
            _weights[i] += _inputs[i].Output * delta;
        }
    }

    public void Respond() {
        float sum = 0f;

        // input of each layer is related to
        // weighted sum of outputs from preceding layer
        for (int i = 0; i < _inputs.Length; i++) {
            sum += _inputs[i].Output * _weights[i];
        }

        _output = Sigmoid(sum);
        _error = 0f;
    }

    float Sigmoid(float f) {
        if (_sigmoid == null) {
            InitSigmoid();
        }
        return _sigmoid[Clamp((int) Math.Floor((f + 5.0) * 20.0), 0, 199)];
    }

    public void InitSigmoid() {
        _sigmoid = new float[200];
        for (int i = 0; i < 200; i++) {
            float x = (i / 20f) - 5f;
            _sigmoid[i] = (float) (2f / (1f + Math.Exp(-bias * x)) - 1f);
        }
    }

    int Clamp(int v, int min, int max) {
        return (v < min) ? min : (v > max) ? max : v;
    }

    public void SetError(float v) {
        _error = v - _output;
    }
}