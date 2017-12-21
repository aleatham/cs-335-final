public class Network {
    // hard-coding layers for this to simplify
    public Neuron[] _input_layer;
    public Neuron[] _hidden_layer;
    private Neuron[] _output_layer;
    public Neuron[] Outputs { get { return _output_layer; } }

    public Network(int inputs, int hidden, int outputs) {
        _input_layer = new Neuron[inputs];
        _hidden_layer = new Neuron[hidden];
        _output_layer = new Neuron[outputs];

        for (int i = 0; i < _input_layer.Length; i++) {
            _input_layer[i] = new Neuron();
        }

        for (int i = 0; i < _hidden_layer.Length; i++) {
            _hidden_layer[i] = new Neuron(_input_layer);
        }

        for (int i = 0; i < _output_layer.Length; i++) {
            _output_layer[i] = new Neuron(_hidden_layer);
        }
    }

    public Network Copy() {
        Network n = new Network(_input_layer.Length, _hidden_layer.Length, _output_layer.Length);

        for (int i = 0; i < _input_layer.Length; i++) {
            n._input_layer[i] = _input_layer[i].Copy();
        }

        for (int i = 0; i < _hidden_layer.Length; i++) {
            n._hidden_layer[i] = _hidden_layer[i].Copy();
        }

        for (int i = 0; i < _output_layer.Length; i++) {
            n._output_layer[i] = _output_layer[i].Copy();
        }

        return n;
    }

    public Neuron[] Neurons() {
        int tLength = _hidden_layer.Length + _output_layer.Length;
        Neuron[] neurons = new Neuron[tLength];
        for (int i = 0; i < tLength; i++) {
            if (i < _hidden_layer.Length)
                neurons[i] = _hidden_layer[i];
            else
                neurons[i] = _output_layer[i - _hidden_layer.Length];
        }

        return neurons;
    }

    public void Respond(float[] inputs) {
        for (int i = 0; i < _input_layer.Length; i++) {
            _input_layer[i].Output = inputs[i];
        }

        for (int i = 0; i < _hidden_layer.Length; i++) {
            _hidden_layer[i].Respond();
        }

        for (int i = 0; i < _output_layer.Length; i++) {
            _output_layer[i].Respond();
        }
    }

    public void Train(float[] outputs) {
        // adjust output layer
        for (int i = 0; i < _output_layer.Length; i++) {
            _output_layer[i].SetError(outputs[i]);
            _output_layer[i].Train();
        }

        // back propagation to hidden layer
        for (int i = 0; i < _hidden_layer.Length; i++) {
            _hidden_layer[i].Train();
        }
    }
}