# Projeto Integrador 3

## Setup

- 6 - Quantidade de atendentes
- RP: AA BB C DD EE - Relação dos postos de trabalho (Numero de guichês)
- RA: ABCDDE (Relação de Atendentes em cada posto)
- Troca: 3 - Custo de uma troca (caso uma fila esteja sobrecarregada), é definido em **TURNOS**

O Sistema é dado em turnos, turnos controlados, ex: 1 turno = 1s ou 1m etc..
### Setup padrão dado em turnos:
```
A - 2
B - 5
C - 7
D - 3
E - 1
```

### Exemplo de arquivo que irá ser lido

U1C5ABDE

- U1 - ID
- C5 - Turno que irá entrar (Chegada 5)
- ABDE - Ordem dos guichês que irá passar até sair do sistema

### Output
```
Tempo médio de um usuário
Tempo médio por tipo de fila
usuário que ficou mais tempo no sistema
tempo médio por combinação ordenado (para ordem de atendimento ABDE por exemplo)
```

## Wireframe de interface

![alt tag](https://github.com/femoreti/Bcc3/blob/master/exemplo%20de%20interface.jpg)
