# MVC-Paginacao-JQUERY
Exemplo de Paginação Next + Previous JQUERY

## Action para buscar os dados JSON

```csharp
  public ActionResult Dados(int? page = 1)
  {
      if (page <= 0)
          return JsonGet(new Data { Success = false, Message = "Informe uma pagina maior ou igual a 1" });

      var count = Db.Documents.Count(d => d.DocumentTypeId == 0);

      if (page > count)
          return JsonGet(new Data { Success = false, Message = $"Informe uma pagina menor que {count}" });

      var documents = Db.Documents.Where(d => d.DocumentTypeId == 0).Skip(page.Value - 1).Take(1);
      var dados = new Data
      {
          Success = true,
          Message = "Sucesso",
          Document = documents.FirstOrDefault(),
          Total = count,
          Page = page.Value
      };
      return JsonGet(dados);
  }
```

## View exibe dados e busca informações com JQUERY

```cshtml
<div class="data">
    <label>Name:</label> 

    <p class="id-documento"></p>
    <p class="nome-documento"></p>
    <input id="idTipoDocumento" name="idTipoDocumento" />
    <button id="save">Atualizar</button>
    <hr />
    <iframe id="iframe" width="100%" height="300px"></iframe>
    <hr />
    <button id="prev" disabled>Previous</button>
    <button id="next" disabled>Next</button>
</div>
<div id="error" style="color:red">

</div>

@section scripts {
    <script>
        let data;
        $(() => {
             
            function request(page) {
                $.ajax({
                    url: '/Home/dados?page=' + page,
                    method: 'GET'
                }).done((resp) => {
                    data = resp;
                    console.log(data)
                    if (data.Success) {
                        $('#error').html('');
                        renderData();
                    } else {
                        $('#error').html(data.Message);
                    }
                });
            }
            function renderData() {
                let document = data.Document;
                $('.id-documento').html(document.DocumentId);
                $('.nome-documento').html(document.DocumentName);
                $('#idTipoDocumento').val(document.DocumentTypeId);
                $('#iframe').prop('src', document.UrlDocument);
                $('#prev').prop("disabled", data.Page == 1);
                $('#next').prop("disabled", data.Page == data.Total);
            }
            $('#save').click(() => {
                var id = $('#idTipoDocumento').val();
                if (id && id > 0) {
                    let document = data.Document;
                    document.DocumentTypeId = id;
                    $.ajax({
                        url: '/Home/AtualizarRegistro',
                        method: 'POST',
                        data: document
                    }).done((resp) => {
                        console.log(resp);
                        request(data.Page)
                    });
                }
            });
            $('#prev').click(() => {
                if (data && data.Page - 1 >= 1) {
                    request(data.Page - 1);
                }
            });
            $('#next').click(() => {
                if (data && data.Page + 1 <= data.Total) {
                    request(data.Page + 1);
                }
            });
            request(1);
        });
    </script>
  }
```
