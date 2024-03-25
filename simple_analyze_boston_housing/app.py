from flask import Flask, render_template, request, jsonify
from flask_cors import CORS
import pandas as pd
from csv import reader

app = Flask(__name__)
CORS(app)

# Carregando os dados
df = pd.read_csv("dados/HousingData.csv")

"""
Rota principal onde renderiza para o 'index.html'
"""
#@app.route("/")
#def index():
#    return render_template("index.html")

"""
Rota para análise com base nos valores informados
e renderiza para a página 'analysis.html'
"""
@app.route("/analyze", methods=["POST"])
def analyze():
    # Obter parâmetros formulário
    feature = request.form.get("feature")
    # Calcula estatísticas
    mean_value = round(df[feature].mean(), 2)
    median_value = round(df[feature].median(),2)
    max_value = round(df[feature].max(),2)
    min_value = round(df[feature].min(),2)

    # return render_template("analysis.html", feature=feature, mean_value=mean_value, median_value=median_value, max_value=max_value, min_value=min_value)
    return jsonify({
        "feature": feature,
        "mean_value": mean_value,
        "median_value": median_value,
        "max_value": max_value,
        "min_value": min_value
    })

@app.route("/base", methods=["GET"])
def base():
    data = df.to_dict(orient="list")        
    return jsonify(data)

@app.route("/filter", methods=["GET", "POST"])
def filter():
    if request.method == "POST":
        data = request.get_json() 

        required_fields = ["type", "asc", "des"]
        if not all(field in data for field in required_fields):
            return jsonify({"message": "Campos obrigatórios, por favor verificar."})
         
        asc = bool(data.get("asc", False))
        des = bool(data.get("des", False))

        type = data["type"]
        if type not in df.columns:
            return jsonify({"message": f"Cabeçalho '{type}' não existe. Informar novo valor!"})
        
        values = df[type].tolist()
            
        if asc:
            values_in_order = sorted(values)
        if des:
            values_in_order = sorted(values, reverse=True)
        if not asc and not des:
            values_in_order = values
        return jsonify({"response":values_in_order}),200
        
    else:
        return jsonify({"message": "Necessário enviar um method='POST'."})

if __name__ == "__main__":
    app.run(debug=True)